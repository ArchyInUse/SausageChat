using System;
using System.Windows.Data;
using System.CodeDom.Compiler;
using System.Reflection;

namespace WindowsApplication1
{
    public  class JScriptConverter : IMultiValueConverter, IValueConverter
    {
        private delegate object Evaluator(string code, object[] values);
        private static Evaluator evaluator;

        static JScriptConverter()
        {
            string source = 
                @"import System; 

                class Eval
                {
                    public function Evaluate(code : String, values : Object[]) : Object
                    {
                        return eval(code);
                    }
                }";

            CompilerParameters cp = new CompilerParameters();
            cp.GenerateInMemory = true;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (System.IO.File.Exists(assembly.Location))
                    cp.ReferencedAssemblies.Add(assembly.Location);

            CompilerResults results = (new Microsoft.JScript.JScriptCodeProvider())
                .CompileAssemblyFromSource(cp, source);

            Assembly result = results.CompiledAssembly;

            Type eval = result.GetType("Eval");

            evaluator = (Delegate.CreateDelegate(
                typeof(Evaluator),
                Activator.CreateInstance(eval),
                "Evaluate") as Evaluator);
        }

        private bool trap = false;
        public bool TrapExceptions
        {
            get { return this.trap; }
            set { this.trap = true; }
        }

        public object Convert(object[] values, System.Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return evaluator(parameter.ToString(), values);
            }
            catch
            {
                if (trap)
                    return null;
                else
                    throw;
            }
        }

        public object Convert(object value, Type targetType, 
            object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(new object[] { value }, targetType, parameter, culture);
        }


        public object[] ConvertBack(object value, System.Type[] targetTypes,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotSupportedException();
        }
    }
}