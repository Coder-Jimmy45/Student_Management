# GitHub Actions CI/CD Pipeline Documentation

## Overview

This document explains the automated CI/CD pipeline for the Student Management .NET project using GitHub Actions.

## Workflow Configuration

### File Location
- `.github/workflows/dotnet.yml`

### Triggers

The workflow is automatically triggered on:
1. **Push to master branch** - Runs on every commit pushed to master
2. **Pull requests to master** - Runs on every PR opened or updated targeting master

### Environment

- **Runner**: `ubuntu-latest`
- **.NET Version**: 8.0.x

### Workflow Steps

#### 1. Checkout Code
- Uses GitHub Actions' `actions/checkout@v4`
- Fetches the latest source code from the repository

#### 2. Setup .NET 8.0
- Uses `actions/setup-dotnet@v4`
- Installs .NET 8.0 SDK on the runner

#### 3. Restore NuGet Packages
- Command: `dotnet restore`
- Restores all project dependencies from NuGet

#### 4. Build Solution
- Command: `dotnet build --configuration Release --no-restore`
- Compiles the solution in Release mode
- `--no-restore` skips restore as packages were already restored

#### 5. Run Tests
- Command: `dotnet test --configuration Release --no-build --verbosity normal --logger "trx;LogFileName=test-results.trx" --collect:"XPlat Code Coverage"`
- Executes all unit tests
- Generates TRX (Test Results XML) format for test reporting
- Collects code coverage metrics using XPlat Code Coverage
- `--no-build` skips rebuild as solution was already built

#### 6. Upload Test Results (Artifacts)
- Uses `actions/upload-artifact@v4`
- Uploads test results and coverage reports
- Stores artifacts for 30 days
- Always runs (even if tests fail) to preserve results for analysis

#### 7. Publish Test Results
- Uses `EnricoMi/publish-unit-test-result-action@v2`
- Publishes test results as GitHub Check annotations
- Makes test failures visible directly in PR comments
- Provides a summary of passed/failed tests

## Error Handling

The workflow:
- **Fails gracefully** on build errors - Stops and reports failure
- **Fails gracefully** on test failures - Stops and reports which tests failed
- **Preserves artifacts** even on failure - Test results are uploaded regardless of pass/fail
- **Reports test details** - GitHub Check provides detailed test summaries

## Security

The workflow:
- ✅ Contains **no hardcoded secrets**
- ✅ Uses GitHub Actions' built-in permissions model
- ✅ Does not store credentials in the workflow file
- ℹ️ For sensitive configuration, use GitHub Secrets and reference them as `${{ secrets.SECRET_NAME }}`

## Configuration for Sensitive Data

For environment-specific configuration (connection strings, API keys, etc.):

1. **Add Repository Secrets** (GitHub Settings → Secrets and variables → Actions)
2. **Reference in workflow**:
   ```yaml
   - name: Configure settings
     env:
       CONNECTION_STRING: ${{ secrets.DB_CONNECTION_STRING }}
     run: echo $CONNECTION_STRING
   ```

3. For local development, continue using `dotnet user-secrets` as documented in README.md

## Build Artifacts

- **Location**: GitHub Actions → Workflow Run → Artifacts
- **Contents**: Test results, coverage reports
- **Retention**: 30 days
- **Access**: Available to repository members with Actions permission

## Status Badge

Add this badge to your README to display the workflow status:

```markdown
[![.NET CI/CD](https://github.com/Coder-Jimmy45/Student_Management/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Coder-Jimmy45/Student_Management/actions/workflows/dotnet.yml)
```

## Monitoring & Troubleshooting

### View Workflow Runs
1. Go to repository → Actions tab
2. Select ".NET CI/CD" workflow
3. Click on a run to see detailed logs

### Common Issues

| Issue | Solution |
|-------|----------|
| Build fails | Check recent commits, review build logs for compilation errors |
| Tests fail | Review test output, check for broken tests or environment issues |
| Restore fails | Verify NuGet sources are accessible, check network connectivity |
| Artifact upload fails | Ensure sufficient storage quota in GitHub Actions |

### Re-running Workflow

- Click the "Re-run jobs" button on a failed workflow run
- Useful for debugging transient failures (network issues, timeouts, etc.)

## Best Practices

1. **Keep workflow simple and efficient** - Avoid unnecessary steps
2. **Fail fast** - Catch errors early in the pipeline
3. **Test locally first** - Run `dotnet build` and `dotnet test` locally before pushing
4. **Monitor build times** - Keep builds under 15 minutes when possible
5. **Review test coverage** - Use coverage reports to identify gaps
6. **Archive results** - Keep 30-day retention for audit trails

## Future Enhancements

Potential additions to the workflow:
- Code quality analysis (SonarQube, Code Climate)
- Automated deployment to staging/production
- Docker image building and publishing
- Security scanning (SAST/DAST)
- Performance testing
- Publish NuGet packages
- Notification integrations (Slack, Teams)

## References

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [.NET CLI Documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/)
- [dotnet test command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
- [Actions Marketplace](https://github.com/marketplace?type=actions)
