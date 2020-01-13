# Luna
基于Lua虚拟机的C#/Unity热更新系统，提供简洁简洁，高效的C# lua互访接口，可将C#类方便的导出到Lua，支持Unity3D,.netcore和.NetFramework等各个平台

## 主要特点

1.使用Delegate进行反射，不使用Emit和Expression
不用生成大量的Wrap代码，而又能保证高性能，还能在各个平台上使用
2.注册C#类到lua只需要一句话

```
luna.RegisterClass<TestClass>();
```
