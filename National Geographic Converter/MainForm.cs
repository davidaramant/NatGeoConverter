using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        int _numberDone;
        int _totalNumber;

        public MainForm() {
            InitializeComponent();

            ChangeState( UIState.Idle );
        }

        #region UI Properties

        int NumberDone {
            get { return _numberDone; }
            set { SetNumberDone( value ); }
        }

        int TotalNumber {
            get { return _totalNumber; }
            set { SetTotalNumber( value ); }
        }

        #endregion UI Properties

        #region UI Update Code

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
                    break;
                case UIState.Processing:
                    _pickOutputButton.Enabled = false;
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
            _numDoneLabel.Text = _numberDone.ToString();
            _progressBar.Value = _numberDone;
        }

        void SetTotalNumber( int totalNumber ) {
            SetValueSafe( SetTotalNumberUnsafe, totalNumber );
        }

        void SetTotalNumberUnsafe( int totalNumber ) {
            _totalNumber = totalNumber;
            _numTotalLabel.Text = _totalNumber.ToString();
            _progressBar.Maximum = _totalNumber;
        }

        void SetValueSafe<T>( Action<T> setter, T value ) {
            if( InvokeRequired ) {
                BeginInvoke( setter, value );
            } else {
                setter( value );
            }
        }

        #endregion UI Update Code

        private void PickInputButton_Click( object sender, EventArgs e ) {

        }

        private void PickOutputButton_Click( object sender, EventArgs e ) {

        }
    }
}
