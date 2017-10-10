# GGMContext
GGMContext는 GGM Framework에서 IoC 컨테이너 역할을 수행하는 Core 부분입니다.

이는 Spring의 Core(Bean, Context)에서 영감을 얻었으며, 모든 GGM Framework는 GGMContext위에 구현됩니다.

## IoC Container
GGMContext는 DL(Dependency Lookup), DI(Dependency Injection)와 ManagedClass 생성 등을 수행하며, 생성된 Managed Class들의 Life Cycle를 관리하여 IoC Container 역할을 수행합니다.

### Dependency Lookup
프레임워크 실행 시 Application이 Run되며, 이는 ApplicationContext를 생성합니다. ApplicationContext는 생성될때 실행한 주체의 Namespace를 얻어온 뒤, 하위 네임스페이스들의 클래스를 스캔하며 ManagedClass로 정해진 객체들을 Lookup 합니다.

### Dependency Injection
GGMContext는 Setter Injection을 지원하지 않으므로, 오직 Constructor Injection만을 사용합니다. 이는 보다 명확하기 떄문입니다.

> Spring에서의 Context는 Bean[Factory]와의 연관을 빼 놓을 수 없지만, GGMContext는 Bean[Factory]에 해당하는 부분을 전부 '코드에 Attribute를 지정함으로서' 대체합니다. 그러므로 굳이 Bean이나 BeanConfiguration class를 작성할 필요가 없습니다.

### Managed Class
GGMContext에 의해 생성되고, 관리되는 객체의 클래스를 ManagedClass라고 합니다.
이는 ApplicationContext의 Scan의 대상이 됩니다. ManagedClass가 지정된 클래스들은 Lookup되어 DI에 사용됩니다.

## Example
```cs
namespace Demo
{
    public class Program
    {
        static void Main(string[] args)
        {
            // 프레임워크 실행, Context가 생성되며 하위 네임스페이스의 Managed Class를 생성하여 Lookup한다.
            Application.Run(typeof(Program));
        }
    }

    [Controller] // ApplicationContext에 의해 생성되어 Lookup된다.
    public class TestController
    {
        // ApplicationContext에 의해 생성될때 인자들을 주입받는다.
        [AutoWired]
        public TestController(TestService testService, TestManagedClass dummy)
        {
            /** */
        }
    }

    [Managed] // ApplicationContext에 의해 생성되어 Lookup된다.
    public class TestManagedClass
    {
        /** */
    }

    [Service] // ApplicationContext에 의해 생성되어 Lookup된다.
    public class TestService
    {
        /** */
    }
}
```

## 추후 작업 목표
- [ ] ManagedClass 지정시 여러 LifeCycle Type 지정 가능하게끔
  - Proto(주입 될 때 마다 생성), Connection(한 Connection에서만) 등