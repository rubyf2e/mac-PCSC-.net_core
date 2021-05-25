using System;
using System.Collections.Generic;

using AppKit;
using Foundation;
using PCSC;

namespace CardReader
{

    public partial class ViewController : NSViewController
    {

        private String CardReaderStatusWording = "讀卡機無連線";

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CardstatusLabel.StringValue = CardReaderStatusWording;
            CardReaderStatusWording     = PrintReaderState();
            CardstatusLabel.StringValue = CardReaderStatusWording;
        }



        private string PrintReaderState()
        {
            var ReaderStatus   = "目前讀卡機狀態：";
            var contextFactory = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System))
            {

                var readerNames = context.GetReaders();

                if (IsEmpty(readerNames))
                {
                    ReaderStatus += "讀卡機未插入";
                    return ReaderStatus;
                }

                foreach (var readerName in readerNames)
                {
                    ReaderStatus += "\r" + readerName;
                }
            }

            return ReaderStatus;
        }



        private string CardStatus()
        {
            var ReaderCardStatus = PrintReaderState();
            var contextFactory   = ContextFactory.Instance;
            using (var context = contextFactory.Establish(SCardScope.System)) {
                var readerNames = context.GetReaders();
                return ReaderCardStatus + DisplayReaderStatus(context, readerNames);
            }
        }


        private string DisplayReaderStatus(ISCardContext context, IEnumerable<string> readerNames)
        {

            var DisplayReaderStatus = "";
            foreach (var readerName in readerNames)
            {

                try
                {
                    using (var reader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
                    {
                        DisplayReaderStatus += PrintReaderStatus(reader);
                    }

                }
                catch (Exception exception)
                {
                    return "\r\r卡片未插入";
                }
            }

            return DisplayReaderStatus;

        }


        private string PrintReaderStatus(ICardReader reader)
        {
            var PrintReaderStatus = "\r\r";

            try
            {
                var status = reader.GetStatus();
                PrintReaderStatus += "序列槽: " + status.Protocol + "\r狀態: " + status.State;
                PrintReaderStatus += PrintCardAtr(status.GetAtr());
            }
            catch (Exception exception)
            {

                return "\r無法讀取卡片\rError message: " + exception  + exception.GetType();
            }

            return PrintReaderStatus;
        }

        private static bool IsEmpty(ICollection<string> readerNames) => readerNames == null || readerNames.Count < 1;

        private string PrintCardAtr(byte[] atr)
        {
            if (atr == null || atr.Length <= 0)
            {
                return "";
            }

            return "\r卡片ATR內碼:\r" + BitConverter.ToString(atr) + "\r";

        }



        partial void readCardClick(Foundation.NSObject sender)
        {
            CardstatusLabel.StringValue = "\r\r讀取中...";
            CardstatusLabel.StringValue = CardStatus();
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }



    }
}


