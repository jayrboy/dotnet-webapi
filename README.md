# Create a web project

```bash
dotnet new webapi --use-controllers -o WebApi
cd WebApi
dotnet add package Microsoft.EntityFrameworkCore.InMemory
code -r ../WebApi
```

```bash
touch .gitignore README.md
```

- .gitignore for untracked directory & files ( bin, obj )
- README.md for written documentation

# Initial Github

- Create a repository : dotnet-webapi to https://github.com/new

```bash
git init
git add .
git commit -m "first commit"
git branch -M main
git remote add origin https://github.com/jayrboy/dotnet-webapi.git
git push -u origin main
```
