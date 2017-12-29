# GGMContext
GGMContext는 GGM Framework에서 DI와 IoC 컨테이너 역할을 수행하는 Core 부분입니다.

이는 Spring의 Core(Bean, Context)에서 영감을 얻었으며, 모든 GGM Framework는 GGMContext위에 구현됩니다.

## IoC Container
GGMContext는 DL(Dependency Lookup), DI(Dependency Injection)와 ManagedClass 생성 등을 수행하며, 생성된 Managed Class들의 Life Cycle를 관리하여 IoC Container 역할을 수행합니다.

### Dependency Lookup
프레임워크 실행 시 Application이 Run되며, 이는 ApplicationContext를 생성합니다. ApplicationContext는 생성될때 실행한 주체의 Namespace를 얻어온 뒤, 하위 네임스페이스들의 클래스를 스캔하며 ManagedClass로 정해진 객체들을 Lookup 합니다.

### Dependency Injection
GGMContext는 Setter Injection을 지원하지 않으므로, 오직 Constructor Injection만을 사용합니다. 이는 보다 명확하기 떄문입니다.

> Spring에서의 Context는 Bean[Factory]와의 연관을 빼 놓을 수 없지만, GGMContext는 Bean[Factory]에 해당하는 부분을 전부 '코드에 Attribute를 지정함으로서' 대체합니다. 그러므로 굳이 Bean이나 BeanConfiguration class를 작성할 필요가 없습니다.

> **IL.Emit**
DI 시 (Reflaction을 이용한)동적 메소드 호출의 성능 저하를 최소화 하기 위해 IL.Emit을 사용하여 동적으로 메소드를 만들어  컴파일 한 뒤 호출합니다..

### Managed Class
GGMContext에 의해 생성되고, 관리되는 객체의 클래스를 ManagedClass라고 합니다.
이는 ApplicationContext의 Scan의 대상이 됩니다. ManagedClass가 지정된 클래스들은 Lookup되어 DI에 사용됩니다.

## Example
```cs

[Managed(ManagedClassType.Singleton)] // ApplicationContext에 의해 생성되어 Lookup된다.
public class TestController
{
    // ApplicationContext에 의해 생성될때 인자들을 주입받는다.
    // TestManagedClass는 Singleton이기 때문에 별다른 생성 없이 룩업 테이블에서 가져와 주입된다.
    // TestService는 Proto이기 때문에 매 주입 시 마다 새로 생성된다.
    [AutoWired]
    public TestController(SingletonManaged singletonManaged, ProtoManaged protoManaged)
    {
        /** */
    }

    SingletonManaged Singleton { get; set; }
    ProtoManaged Proto { get; set; }
}

[Managed(ManagedClassType.Singleton)]
public class SingletonManaged
{
    /** */
}

[Managed(ManagedClassType.Proto)]
public class ProtoManaged
{
    /** */
}

public static void Main(string[] args)
{
    var context = new ManagedContext();
    var singleto = context.GetManaged<SingletonManaged>();
    var proto = context.GetManaged<ProtoManaged>();
    var testController = context.GetManaged<TestController>();

    Console.WriteLine(testController.Singleton == singleton); // true
    Console.WriteLine(testController.proto == proto); // false
}

```
