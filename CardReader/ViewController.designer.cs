// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CardReader
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextFieldCell CardstatusLabel { get; set; }

		[Action ("readCardClick:")]
		partial void readCardClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CardstatusLabel != null) {
				CardstatusLabel.Dispose ();
				CardstatusLabel = null;
			}
		}
	}
}
