using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace National_Geographic_Converter
{
    static class Program
    {
		static void Main()
        {
			string inputPath = @"/Users/davidaramant/Desktop/CNG App 1.66b1 251/assets/fixes";
			string outputPath = @"/Users/davidaramant/Desktop/Fixed Pages";

			CngConverter.ConvertAllInPath( inputPath, outputPath );
        }
    }
}
