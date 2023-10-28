using SmartA4;

// USB口电源控制，可用于控制外部USB设备上电。如风扇、灯光、水泵等
var usb = new OutputPort { FileName = "/dev/usbpwr" };

// 上电
usb.Write(true);

Thread.Sleep(500);

// 断电
usb.Write(false);

Thread.Sleep(500);
