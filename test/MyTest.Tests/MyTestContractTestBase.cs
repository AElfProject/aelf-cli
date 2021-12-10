using AElf.Boilerplate.TestBase;
using AElf.Cryptography.ECDSA;

namespace MyTest
{
    public class MyTestContractTestBase : DAppContractTestBase<MyTestContractTestModule>
    {
        // You can get address of any contract via GetAddress method, for example:
        // internal Address DAppContractAddress => GetAddress(DAppSmartContractAddressNameProvider.StringName);

        internal MyTestContractContainer.MyTestContractStub GetMyTestContractStub(ECKeyPair senderKeyPair)
        {
            return GetTester<MyTestContractContainer.MyTestContractStub>(DAppContractAddress, senderKeyPair);
        }
    }
}