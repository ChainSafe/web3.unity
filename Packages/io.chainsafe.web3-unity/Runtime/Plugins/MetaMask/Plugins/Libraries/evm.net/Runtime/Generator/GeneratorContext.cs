using System.Collections.Generic;
using evm.net.Models.ABI;

namespace evm.net.Generator
{
    public class GeneratorContext
    {
        public string ContractName { get; private set; }
        private List<CodeGenerator> generators = new List<CodeGenerator>();
        private HashSet<string> tupleTypes = new HashSet<string>();
        public string RootNamespace { get; private set; }

        public IList<CodeGenerator> Generators
        {
            get
            {
                return generators.AsReadOnly();
            }
        }

        public GeneratorContext(string rootNamespace, string contractName)
        {
            RootNamespace = rootNamespace;
        }

        public bool HasTupleType(string internalType)
        {
            return tupleTypes.Contains(internalType);
        }

        public void GenerateTuple(string internalType, ABIParameter parameter)
        {
            if (HasTupleType(internalType))
                return;

            var codeGenerator = new TupleTypeGenerator(parameter, this, internalType);
            tupleTypes.Add(internalType);
            AddGenerator(codeGenerator);
        }

        public void AddGenerator(CodeGenerator generator)
        {
            generators.Add(generator);
        }
    }
}