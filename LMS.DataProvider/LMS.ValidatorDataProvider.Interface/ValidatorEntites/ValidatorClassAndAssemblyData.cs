namespace LMS.ValidatorDataProvider.Interface.ValidatorEntites
{


    /// <summary>
    /// Class containing the information required for finding and instantiating a ValidatorClass
    /// </summary>
    public class ValidatorClassAndAssemblyData
    {
        public string ClassName;
        public string AssemblyName;

        public ValidatorClassAndAssemblyData(string className, string assemblyName)
        {
            ClassName = className;
            AssemblyName = assemblyName;
        }
    }
}
