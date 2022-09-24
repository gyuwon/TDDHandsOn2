# 현실 세상의 TDD 실전편 실습 코드

Fast campus 강의 ['현실 세상의 TDD 실전편'](https://fastcampus.co.kr/dev_red_ygw2) 실습에 사용된 예제 코드를 제공합니다.

> 예제 코드는 강의 촬영 전에 미리 준비되었고 강의 촬영 시 라이브 코딩이 진행되었기 때문에 세부 코드는 강의 영상에서 보는 것과 다를 수 있습니다.

## 질문 및 토론

TDD 또는 강의와 관련된 질문과 토론을 위해 Discord 서버를 만들었습니다. 여기 [**초대 링크**](https://discord.gg/NjC9r6kvUz)를 통해 들어오실 수 있습니다. 이 저장소에 이슈를 등록해주셔도 됩니다. 두 소통 채널은 각자 장단점이 있으니 주제에 따라 편하게 적합한 수단을 선택해 주세요.

## 세션 별 코드

태그를 통해 실습이 포함된 각 세션 별 실습 진행에 기반이되는 코드와 실습이 완료된 코드를 볼 수 있습니다.

[홈으로](../../)

### 챕터 2. 주문 및 정산 기능 개선

| 세션 | 기반 코드 |
| - | :-: |
| 1. 작은 변경들 | [`2-1`](../../tree/2-1) |
| 2. 리팩터링 | [`2-2`](../../tree/2-2) |
| 3. 읽기 쉬운 테스트 코드 | [`2-3`](../../tree/2-3) |
| 4. 비동기 프로세스 | [`2-4`](../../tree/2-4) |
| 5. 매개변수화 테스트 | [`2-5`](../../tree/2-5) |
| 6. (Bonus) Mac에서 예제 구동 | [`2-6`](../../tree/2-6) |

### 챕터 3. B2B 계정관리 기능 개선

| 세션 | 기반 코드|
| - | :-: |
| 1. 코드 분리 | [`3-1`](../../tree/3-1) |
| 2. 테스트 환경 격리 | [`3-2`](../../tree/3-2) |
| 3. 모델 정제 | [`3-3`](../../tree/3-3) |
| 4. 모델 통합 | [`3-4`](../../tree/3-4) |
| 5. 모델 확장 | [`3-5`](../../tree/3-5) |

## 개발 환경

### Docker

https://docs.docker.com/get-docker/

### 데이터베이스

PostgreSQL을 데이터베이스로 사용합니다.

https://hub.docker.com/_/postgres

```text
docker run --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgres
```

### Azure Storage Queue

메시지 중개자로 Azure Storage Queue를 사용합니다.

https://hub.docker.com/_/microsoft-azure-storage-azurite

```text
docker run --name azurite -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite
```

### Java 17 및 빌드 도구

- https://www.oracle.com/java/technologies/downloads/#java17
- https://gradle.org/install/

### .NET 6

https://dotnet.microsoft.com/en-us/download/dotnet/6.0

### IDE 또는 편집기

#### Windows

- [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
- [IntelliJ IDEA Community](https://www.jetbrains.com/idea/download/)

#### Mac

- [Visual Studio Code](https://code.visualstudio.com/download)

##### Visual Studio Code 확장

- [C# for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [Extension Pack for Java](https://marketplace.visualstudio.com/items?itemName=vscjava.vscode-java-pack)
