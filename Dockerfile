FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY src/DemoSurvey.sln ./
COPY src/**/*.csproj ./

RUN for f in *.csproj; do \
    filename=$(basename $f) && \
    dirname=${filename%.*} && \
    mkdir $dirname && \
    mv $filename ./$dirname/; \
    done

RUN dotnet restore --no-cache

COPY src ./

RUN dotnet publish DemoSurvey.Web -c release -o /published --no-restore 

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /published ./
ENTRYPOINT ["dotnet", "DemoSurvey.Web.dll"]