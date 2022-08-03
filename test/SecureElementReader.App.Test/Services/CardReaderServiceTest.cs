using Moq;
using Moq.AutoMock;
using PCSC;
using PCSC.Iso7816;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using SecureElementReader.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecureElementReader.App.Test.Services
{
    public class CardReaderServiceTest
    {
        private readonly AutoMocker _autoMocker;

        public CardReaderServiceTest()
        {
            _autoMocker = new AutoMocker();
        }

        [Fact]
        public void Verify_Pin_Test()
        {           
            var command = new CommandApdu(IsoCase.Case2Extended, PCSC.SCardProtocol.Any);
            var response = new Response(
                  new List<ResponseApdu>
                  {
                       new ResponseApdu(new byte[] {0x90, 0x90}, 2, IsoCase.Case1, PCSC.SCardProtocol.T1),
                        new ResponseApdu(new byte[] {0x90, 0x90}, 2, IsoCase.Case1, PCSC.SCardProtocol.T1),
                  },
                  new List<PCSC.SCardPCI>
                  {
                      new PCSC.SCardPCI(PCSC.SCardProtocol.T1, 0)
                  });
            _autoMocker.Setup<IApduCommandService, CommandApdu>(s => s.SelectPKIApp())
                .Returns(command)
                .Verifiable();
            _autoMocker.Setup<IIsoReader, Response>(s => s.Transmit(command)).Returns(response);

            _autoMocker.Setup<IApduCommandService, CommandApdu>(s => s.VerifyPkiPin(new byte[] { 0x01, 0x02 })).Returns(command);

            //var a = new Mock<ISCardContext>();
            //a.Setup(s => s.GetReaders()).Returns(new[] { "Gemalto" });

            //var b = new Mock<IContextFactory>();
            //b.Setup(s => s.Establish(SCardScope.System)).Returns(a.Object);

            //var r = new Mock<CardReaderService>();


            var service = _autoMocker.CreateInstance<CardReaderService>(true);            
            var result = service.VerifyPin("1234");
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("123")]
        public void TestVerifyPin(string pin)
        {
            var service = _autoMocker.CreateInstance<CardReaderService>();
            var result = service.VerifyPin(pin);

        }
    }
}
