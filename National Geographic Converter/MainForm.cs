using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace National_Geographic_Converter {
    public partial class MainForm : Form {

        enum UIState {
            Idle,
            InputChosen,
            Processing,
            Done
        }

        private readonly object _numberDoneLock = new object();

        int _numberDone;
        int _totalNumber;

        string _inputPath;
        string _outputPath;

        private readonly BackgroundWorker _worker = new BackgroundWorker();

        public MainForm() {
            InitializeComponent();

            _worker.DoWork += Worker_DoWork;

            ChangeState( UIState.Idle );
        }

        #region UI Properties

        int NumberDone {
            get {
                lock( _numberDoneLock ) {
                    return _numberDone;
                }
            }
            set {
                lock( _numberDoneLock ) {
                    SetNumberDone( value );
                }
            }
        }

        int TotalNumber {
            get { return _totalNumber; }
            set { SetTotalNumber( value ); }
        }

        #endregion UI Properties

        #region UI Code

        void ChangeState( UIState state ) {
            SetValueSafe( ChangeStateUnsafe, state );
        }

        void ChangeStateUnsafe( UIState state ) {
            switch( state ) {
                case UIState.Idle:
                    NumberDone = 0;
                    TotalNumber = 0;
                    _pickOutputButton.Enabled = false;
                    break;
                case UIState.InputChosen:
                    _pickInputButton.Enabled = false;
                    _pickOutputButton.Enabled = true;
                    _inputPathTextBox.Text = _inputPath;
                    break;
                case UIState.Processing:
                    _pickOutputButton.Enabled = false;
                    _outputPathTextBox.Text = _outputPath;

                    _worker.RunWorkerAsync();
                    break;
                case UIState.Done:
                default:
                    break;
            }
        }

        void SetNumberDone( int numberDone ) {
            SetValueSafe( SetNumberDoneUnsafe, numberDone );
        }

        void SetNumberDoneUnsafe( int numberDone ) {
            _numberDone = numberDone;
            _numDoneLabel.Text = _numberDone.ToString( "###,###,###,##0" );
            _progressBar.Value = _numberDone;
        }

        void SetTotalNumber( int totalNumber ) {
            SetValueSafe( SetTotalNumberUnsafe, totalNumber );
        }

        void SetTotalNumberUnsafe( int totalNumber ) {
            _totalNumber = totalNumber;
            _numTotalLabel.Text = _totalNumber.ToString( "###,###,###,##0" );
            _progressBar.Maximum = _totalNumber;
        }

        void SetValueSafe<T>( Action<T> setter, T value ) {
            if( InvokeRequired ) {
                BeginInvoke( setter, value );
            } else {
                setter( value );
            }
        }

        private void PickInputButton_Click( object sender, EventArgs e ) {
            using( var chooser = new FolderBrowserDialog() ) {
                chooser.ShowNewFolderButton = false;
                if( chooser.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
                    _inputPath = Path.GetFullPath( chooser.SelectedPath );
                    ChangeState( UIState.InputChosen );
                }
            }
        }

        private void PickOutputButton_Click( object sender, EventArgs e ) {
            using( var chooser = new FolderBrowserDialog() ) {
                chooser.ShowNewFolderButton = true;
                if( chooser.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
                    _outputPath = Path.GetFullPath( chooser.SelectedPath );
                    ChangeState( UIState.Processing );
                }
            }
        }

        #endregion UI Code

        void Worker_DoWork( object sender, DoWorkEventArgs e ) {
            var allFiles = Directory.EnumerateFiles( _inputPath, "*.cng", SearchOption.AllDirectories ).ToArray();

            NumberDone = 0;
            TotalNumber = allFiles.Length;

            foreach( var batch in allFiles.InSetsOf( 100 ) ) {
                var oldAndNewNames = batch.AsParallel().Select( name => new { 
                        Old = name, 
                        New = GetOutputFilePath( name ) } );

                var loadedFiles =
                    from namePair in oldAndNewNames
                    where !File.Exists( namePair.New )
                    let data = File.ReadAllBytes( namePair.Old )
                    select new {
                        OutputName = namePair.New,
                        Data = data,
                    };

                var fixedDataFiles =
                    loadedFiles.AsParallel().Select( loadedFile => new { 
                        Name = loadedFile.OutputName, 
                        Data = loadedFile.Data.Select( b => (byte)( b ^ 0xEF ) ).ToArray() } );

                foreach( var fixedFile in fixedDataFiles ) {
                    File.WriteAllBytes( fixedFile.Name, fixedFile.Data );
                }

                NumberDone += batch.Count();
            }

            ChangeState( UIState.Done );
        }

        private string GetOutputFilePath( string inputFilePath ) {
            var components = inputFilePath.Split( new[] { Path.DirectorySeparatorChar } ).ToArray();

            var eraPath = components[components.Length - 3];
            var issuePath = components[components.Length - 2];
            var fileName = Path.GetFileNameWithoutExtension( components[components.Length - 1] );

            var fullEraPath = Path.Combine( _outputPath, eraPath );

            if( !Directory.Exists( fullEraPath ) ) {
                Directory.CreateDirectory( fullEraPath );
            }

            var fullIssuePath = Path.Combine( fullEraPath, issuePath );

            if( !Directory.Exists( fullIssuePath ) ) {
                Directory.CreateDirectory( fullIssuePath );
            }

            return Path.Combine( fullIssuePath, fileName + ".jpg" );
        }
    }
}
