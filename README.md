SqrlNet
=======

This is a .NET implementation of Steve Gibson's SQRL secure login protocol (https://www.grc.com/sqrl/sqrl.htm).  It is written with the concepts of Inversion of Control and Dependency Injection in mind.  This way, individual implementations of cryptography can be swapped out, and unit tests can be specific to SqrlNet, and be crypto-agnostic.

The cryptographic functionality has been broken up into three parts; SCrypt Password Based Key Derivation Function (PBKDF), SHA256 Hash-based Message Authentication Code (HMACSHA256), and eliptic curve asymetric key cryptography (ed25519).  Each of these parts is implemented separately in its own class, and is abstracted by an interface.  Anyone wishing to use this library can supply their own implentation of any interface, or use the provided implementations.  This separation is important because the implementations of other cryptographic libraries are ever-changing, and it is not wise to tie a SQRL implementation to any one of them.  In fact, two thrid-party crypto libraries (libsodium-net, and CryptSharp) are currently used to implement the needed functionality, as well as the .NET framework's standard cryptography implementations.

### Demos

[SQRL client and server running in Ubuntu](http://www.youtube.com/watch?v=UQAUVLpb1pU)

[SqrlNet in development with the client running](http://www.youtube.com/watch?v=Kp1MJFE0fBM)
