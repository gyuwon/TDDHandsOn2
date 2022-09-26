# 현실 세상의 TDD 실전편 실습 코드

Fast campus 강의 ['현실 세상의 TDD 실전편'](https://fastcampus.co.kr/dev_red_ygw2) 실습에 사용된 예제 코드를 제공합니다.

> 예제 코드는 강의 촬영 전에 미리 준비되었고 강의 촬영 시 라이브 코딩이 진행되었기 때문에 세부 코드는 강의 영상에서 보는 것과 다를 수 있습니다.

## 질문 및 토론

TDD 또는 강의와 관련된 질문과 토론을 위해 Discord 서버를 만들었습니다. 여기 [**초대 링크**](https://discord.gg/NjC9r6kvUz)를 통해 들어오실 수 있습니다. 이 저장소에 이슈를 등록해주셔도 됩니다. 두 소통 채널은 각자 장단점이 있으니 주제에 따라 편하게 적합한 수단을 선택해 주세요.

## 세션 별 코드

태그를 통해 실습이 포함된 각 세션 별 실습 진행에 기반이되는 코드를 볼 수 있습니다.

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

예제 코드를 실행하고 실습하기 위해 다음 도구들이 설치되어 있어야 합니다.

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

### 데이터베이스 클라이언트

- [pgAdmin 4](https://www.pgadmin.org/download/)

## 응용프로그램 실행

Mac과 Windows에서 예제 응용프로그램을 실행하는 방법을 설명합니다.

> 먼저 '개발 환경' 섹션을 참고해 PostgreSQL 서버와 Azurite가 구동시키고, JDK 17, Gradle, .NET 6.0을 설치해주세요.

> 설명된 모든 CLI 명령 실행은 코드 저장소 루트 디렉터리가 기준입니다. 다른 디렉터리에서 명령을 실행하려면 명령 인자를 수정해야 합니다.

### 데이터베이스 마이그레이션

Entity Framework 도구를 설치합니다.

```text
dotnet tool install dotnet-ef --global
```

Orders 서비스 데이터베이스를 마이그레이션 합니다.

```text
dotnet ef database update --project ./ShopPlatform/Orders/Orders.Api/
```

Sellers 서비스 데이터베이스를 마이그레이션 합니다.

```text
dotnet ef database update --project ./ShopPlatform/Sellers/Sellers.Sql/ --startup-project ./ShopPlatform/Sellers/Sellers.Api/
```

### Orders 서비스 실행

Orders API 응용프로그램을 구동합니다.

```text
dotnet run --project ./ShopPlatform/Orders/Orders.Api/
```

Orders API Swagger 문서에 접속합니다.

http://localhost:5094/swagger/index.html

### Sellers 서비스 실행

Sellers API 응용프로그램을 구동합니다.

```text
dotnet run --project ./ShopPlatform/Sellers/Sellers.Api/
```

Sellers API Swagger 문서에 접속합니다.

http://localhost:5232/swagger/index.html

### Accounting 서비스 실행

```text
./ShopPlatform/gradlew bootRun -p ./ShopPlatform/
```

Accounting API Swagger 문서에 접속합니다.

http://localhost:1579/swagger-ui/index.html
