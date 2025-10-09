# WzPipeline

WzPipeline은 메이플스토리 WZ 파일에서 다양한 게임 데이터를 추출하여 JSON 형태로 변환하거나 아이콘 이미지를 추출하는 도구입니다.
[malib](https://github.com/Lechros/malib)의 리소스 제작을 위해 사용됩니다.

## 사용법

### 환경

- Windows
- .NET 8.0
- Node.js 및 pnpm
- Base.wz
- 2GB 이상의 여유 RAM

### 1. Wz 파일 경로 설정

`WzPipeline/Cli/Program.cs`에서 WZ 파일 경로를 설정합니다:

```csharp
private const string BaseWzPath = @"C:\Nexon\Maple\Data\Base\Base.wz";
```

### 2. 빌드 및 실행

```bash
dotnet restore

dotnet build

dotnet run --project WzPipeline
```

## 아키텍처

WzPipeline은 다음과 같은 단계로 데이터를 처리합니다:

1. **Traverser**: WZ 파일에서 노드를 순회하며 데이터 추출
2. **Converter**: 추출된 데이터를 도메인 모델로 변환
3. **Processor**: 변환된 데이터를 추가 처리
4. **Exporter**: 최종 데이터 내보내기

`INode`는 Wz_Node 접근을 추상화하는 계층이며 노드 포인터 외의 상태를 가지지 않습니다.
다음 파이프라인에 전달할 때 반드시 일반 모델 객체로 변환해야 합니다.

최대한 많은 의존성을 Converter에서 처리합니다. 의존성을 여러 클래스에 추가하면 설정이 귀찮기 때문입니다.

Scripts 폴더는 파이프라인에서 malib 라이브러리를 활용하기 위한 레이어입니다.
JsonService를 사용하여 C# 클래스 - JS Object 간 변환을 수행합니다.

새로운 Job 추가는 `Application.DependencyInjection` 및 `Cli.Program`을 수정합니다.
또한 그에 맞는 테스트를 `DependencyInjectionTests`에 추가해야 합니다.
구현 방법은 기존 코드를 참고하세요.
