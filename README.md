SqrlNet
=======

This is a .NET implementation of Steve Gibson's SQRL secure login protocol (https://www.grc.com/sqrl/sqrl.htm).  It is written with the concepts of Inversion of Control and Dependency Injection in mind.  This way, individual implementations of cryptography can be swapped out, and unit tests can be specific to SqrlNet, and be crypto-agnostic.

The cryptographic functionality has been broken up into three parts; SCrypt Password Based Key Derivation Function (PBKDF), SHA256 Hash-based Message Authentication Code (HMACSHA256), and elliptic curve asymmetric key cryptography (ed25519).  Each of these parts is implemented separately in its own class, and is abstracted by an interface.  Anyone wishing to use this library can supply their own implementation of any interface, or use the provided implementations.  This separation is important because the implementations of other cryptographic libraries are ever-changing, and it is not wise to tie a SQRL implementation to any one of them.  In fact, two third-party crypto libraries (libsodium-net, and CryptSharp) are currently used to implement the needed functionality, as well as the .NET framework's standard cryptography implementations.

## Building

Clone the repository using:

	git clone --recursive git@github.com:jestin/SqrlNet.git

This should clone the project as well as all the submodules that this project uses for dependencies (libsodium-net and cryptsharp).  The solution file will include all the .NET projects from the submodules.

Once cloned, you can open SqrlNet.sln from either Visual Studio, MonoDevelop, or Xamarin Studio.  Alternatively, you can build from command line:

	cd SqrlNet
	mdtool build

To clean the projecct, run:

	mdtool build -t:Clean SqrlNet.sln

## Running

In order to run the example applications (SqrlGtk, SqrlServerExample, SqrlNetExample), you need to make sure that libsodium is installed in a directory where it will be available to applications.  This means it must either reside in the `bin` directory of the applications, or a directory in your system's library path (eg. `/usr/local/lib` for Ubuntu).

### Registering the protocol handler

On the desktop, SQRL works by presenting a click-able link to the user in a web browser.  However, instead of navigating to a new URL, the operating system is supposed to launch a SQRL client application to perform the login communication with the web server.  This means that in order to use SQRL, a user's operating system will need to be configured to know where an SQRL client is, how to call it, and when to call it.  For this, we need to register a protocol handler.

On Ubuntu, and other Linux systems, this is as easy as creating a `*.desktop` file in the `/usr/share/applications/` directory.  Since we are going to use this configuration to call the application named `SqrlGtk.exe`, we are going to name the file `sqrlgtk.desktop`.  Use an editor of your choice to create this file, and put the following inside it:

	[Desktop Entry]
	Encoding=UTF-8
	Type=Application
	Terminal=false
	Exec=/home/jestin/Projects/SqrlNet/SqrlGtk/bin/Debug/SqrlGtk.exe %U
	Name[en_US]=SqrlGtk
	Comment[en_US]=Gtk# SQRL client
	Name=SqrlGtk
	Categories=Application;Network;
	MimeType=x-scheme-handler/sqrl;x-scheme-handler/qrl;
	Comment[en_US.utf8]=Gtk# SQRL client

Be sure to change the line that reads `Exec=/home/jestin/Projects/SqrlNet/SqrlGtk/bin/Debug/SqrlGtk.exe %U` to point to wherever your application resides, and also be sure to point to the build you want to use.  I use this to launch my application for debugging purposes, so I prefer to specify the build in my `Debug` directory.  For actual use, users would want to be sure to use the `Release` build.

On Windows systems, follow the guide on [Registering an Application to a URI Scheme](http://msdn.microsoft.com/en-us/library/aa767914(v=vs.85).aspx) to set up your operating system to launch the SQRL client.

OS X users can follow [this guide](https://support.shotgunsoftware.com/entries/127152-launching-external-applications-using-custom-protocols-under-osx) on how to register a protocol handler.

## Demos

Here are some demo videos of my work so far.  Since I started this project before any sort of formal specification was discussed or published, these demos only serve as proofs of concept.  They use the same cryptography that is used in the final specification, but lack much of the full feature set, and certainly don't follow the standards for communication between client and server.  Still, they can give the user a good overview for what a SQRL login will look like.

[SQRL client and server running in Ubuntu](http://www.youtube.com/watch?v=UQAUVLpb1pU)

[SqrlNet in development with the client running](http://www.youtube.com/watch?v=Kp1MJFE0fBM)
