# Estimate Dev Time Command

## Purpose
Provide accurate development time estimates by analyzing code changes, complexity, change types, and adjusting for developer experience and project-specific factors. This command helps with sprint planning, project estimation, and resource allocation.

## When to Use
- **Sprint Planning**: Estimate effort for upcoming tasks and features
- **Project Estimation**: Get time estimates for major development work
- **Change Analysis**: Understand effort required for specific code changes
- **Resource Planning**: Allocate development time and resources effectively
- **Client Communication**: Provide realistic timelines to stakeholders
- **Risk Assessment**: Identify potentially complex or time-consuming changes

## Command Structure

### Phase 1: Change Detection & Analysis
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive analysis of the target changes.

#### Change Target Detection
- **Staged Files**: Analyze files staged for commit (`git diff --cached`)
- **Unstaged Files**: Analyze modified files not yet staged (`git diff`)
- **Specific Files**: Analyze specified file paths
- **Default Behavior**: Analyze unstaged files if no target specified

#### Technology Detection
- **File Extension Analysis**: Identify technologies from file extensions (.ts, .jsx, .vue, .cs, etc.)
- **Import Analysis**: Detect frameworks and libraries from import statements
- **Configuration Detection**: Identify build tools and frameworks from config files
- **Dependency Analysis**: Analyze package.json, requirements.txt, etc.

#### Change Type Classification
- **New Feature**: New functionality, components, or modules
- **Bug Fix**: Error corrections and issue resolutions
- **Refactoring**: Code improvement without changing functionality
- **Migration**: Technology or framework upgrades
- **Auto-Detection**: Analyze patterns to determine change type
- **Manual Override**: Use `--change-type` flag to specify type

#### Complexity Analysis
- **Code Metrics**: Lines added/removed, cyclomatic complexity, nesting depth
- **Dependency Analysis**: Number of dependencies, coupling complexity
- **Test Coverage**: Existing test coverage and gaps
- **Technical Debt**: Code quality issues and maintenance burden

### Phase 2: Experience Profile Integration
**Technology-Adaptive Estimation**: Adjust estimates based on developer expertise levels.

#### Experience Profile Loading
- **Configuration File**: Load from `.cursor/config/experience-profile.json`
- **Technology Mapping**: Map detected technologies to experience levels
- **Multiplier Application**: Apply experience-based time multipliers
- **Fallback Handling**: Use default multipliers for unknown technologies

#### Experience Levels
- **Expert** (0.7x): 5+ years, deep mastery, architectural decisions
- **Proficient** (1.0x): 3-5 years, comfortable with complex patterns
- **Competent** (1.3x): 1-3 years, can implement with guidance
- **Learning** (1.8x): <1 year, requires significant learning time

#### Technology-Specific Adjustments
- **Angular**: Expert level (7 years) - 0.7x multiplier
- **React**: Proficient level (3 years) - 1.0x multiplier
- **Ionic**: Competent level (1 year) - 1.3x multiplier
- **.NET/C#**: Expert level (8 years) - 0.7x multiplier
- **Node.js**: Proficient level (3 years) - 1.0x multiplier
- **TypeScript**: Expert level (7 years) - 0.7x multiplier

### Phase 3: Estimation Algorithm
**Structured Estimation Models**: Apply different estimation models based on change type.

#### Base Estimation Models

**New Feature Model**:
- **Base Time**: 4 hours per component/module
- **Complexity Adjustment**: ×(1 + complexity_score/10)
- **Experience Adjustment**: ×technology_multiplier
- **Testing Time**: +30% unit tests, +20% E2E tests (if `--include-tests` flag)
- **Verification Time**: +15% manual testing and verification

**Bug Fix Model**:
- **Base Time**: 2 hours per issue
- **Severity Adjustment**: ×(1 + severity_level/5)
- **Investigation Time**: +50% for complex bugs requiring deep analysis
- **Experience Adjustment**: ×technology_multiplier
- **Verification Time**: +20% manual testing and regression testing

**Refactoring Model**:
- **Base Time**: 3 hours per module
- **Scope Adjustment**: ×(affected_files/5)
- **Test Update Time**: +40% for updating existing tests
- **Experience Adjustment**: ×technology_multiplier
- **Verification Time**: +10% regression testing

**Migration Model**:
- **Base Time**: 6 hours per major component
- **Breaking Changes**: ×(1 + breaking_changes/3)
- **Learning Curve**: +30% for first component, -10% per subsequent
- **Experience Adjustment**: ×technology_multiplier
- **Testing Time**: +50% comprehensive testing
- **Verification Time**: +25% manual testing and validation

#### Complexity Scoring (1-10 Scale)

**Code Complexity Factors**:
- **Function/Component Size**: Large functions increase complexity
- **Nesting Depth**: Deep nesting increases complexity
- **Dependencies**: High coupling increases complexity
- **Technical Debt**: Existing issues increase complexity
- **Test Coverage**: Low coverage increases complexity

**Change Complexity Factors**:
- **Scope**: Number of files affected
- **Cross-Cutting Concerns**: Changes affecting multiple layers
- **Integration Points**: External API or service dependencies
- **Breaking Changes**: Changes that affect other components

#### Project-Specific Adjustments
- **Technical Debt Factor**: Adjust based on existing code quality
- **Test Coverage Factor**: Adjust based on current test coverage
- **Team Experience**: Adjust for team familiarity with codebase
- **Documentation Quality**: Adjust based on available documentation

### Phase 4: Three-Point Estimation
**Risk-Adjusted Estimates**: Provide optimistic, realistic, and pessimistic estimates.

#### Estimate Ranges
- **Optimistic** (0.8x): Everything goes smoothly, no unexpected issues
- **Realistic** (1.0x): Expected timeline with normal challenges
- **Pessimistic** (1.5x): Account for unknowns, debugging, and complications

#### Risk Factors
- **Technical Risk**: New technologies or complex integrations
- **Integration Risk**: Dependencies on external systems
- **Testing Risk**: Complex testing scenarios or missing test data
- **Documentation Risk**: Poor or missing documentation

### Phase 5: Output Generation
**Comprehensive Reporting**: Generate detailed, actionable estimates.

#### Output Formats

**Summary Format** (Default):
```markdown
## Development Time Estimate

**Change Type**: New Feature
**Files Analyzed**: 3 files
**Technologies**: Angular, TypeScript
**Complexity Score**: 6/10 (Medium)

### Time Estimates
- **Optimistic**: 4.2 hours
- **Realistic**: 5.3 hours  
- **Pessimistic**: 7.9 hours

### Breakdown
- Implementation: 3.2 hours
- Testing: 1.6 hours (if --include-tests)
- Verification: 0.5 hours
- **Total**: 5.3 hours (realistic)
```

**Detailed Format** (`--detailed` flag):
```markdown
## Detailed Development Time Estimate

### File-by-File Breakdown

#### src/components/user-profile.component.ts
- **Type**: New Feature
- **Complexity**: 5/10
- **Base Time**: 2.0 hours
- **Experience Multiplier**: 0.7x (Angular Expert)
- **Adjusted Time**: 1.4 hours
- **Testing**: 0.4 hours
- **Verification**: 0.2 hours
- **Subtotal**: 2.0 hours

#### src/services/user.service.ts
- **Type**: New Feature
- **Complexity**: 7/10
- **Base Time**: 2.0 hours
- **Experience Multiplier**: 0.7x (TypeScript Expert)
- **Adjusted Time**: 1.4 hours
- **Testing**: 0.4 hours
- **Verification**: 0.2 hours
- **Subtotal**: 2.0 hours

### Summary
- **Total Files**: 2
- **Total Time**: 4.0 hours
- **Optimistic**: 3.2 hours
- **Realistic**: 4.0 hours
- **Pessimistic**: 6.0 hours
```

## Command Flags

### Analysis Flags
- `--staged`: Analyze only staged files
- `--unstaged`: Analyze only unstaged files (default)
- `--files <paths>`: Analyze specific files (comma-separated)

### Output Flags
- `--detailed, -d`: Show per-file breakdown with subtask estimates
- `--include-tests, -t`: Include unit and E2E testing time estimates
- `--json`: Output results in JSON format

### Override Flags
- `--change-type <type>`: Override auto-detection (feature|bug|refactor|migrate)
- `--complexity <level>`: Override complexity analysis (low|medium|high)
- `--experience <level>`: Override experience level (expert|proficient|competent|learning)

## Usage Examples

### Basic Usage
```bash
# Estimate unstaged changes
/estimate-dev-time

# Estimate staged changes
/estimate-dev-time --staged

# Estimate specific files
/estimate-dev-time --files src/components/user-profile.component.ts,src/services/user.service.ts
```

### Detailed Analysis
```bash
# Detailed breakdown with testing time
/estimate-dev-time --detailed --include-tests

# Override change type and complexity
/estimate-dev-time --change-type migration --complexity high --detailed
```

### Advanced Usage
```bash
# JSON output for integration
/estimate-dev-time --json --staged

# Override experience level for specific technology
/estimate-dev-time --experience expert --files src/legacy/old-code.js
```

## Estimation Accuracy Guidelines

### Calibration Factors
- **Experience Level**: Higher experience = faster implementation
- **Code Quality**: Better code = easier changes
- **Test Coverage**: Good tests = faster debugging
- **Documentation**: Good docs = less investigation time
- **Team Familiarity**: Known codebase = faster changes

### Common Adjustments
- **New Technology**: Add 50-100% for first implementation
- **Legacy Code**: Add 25-50% for poor documentation/quality
- **Complex Integration**: Add 30-60% for external dependencies
- **Performance Critical**: Add 20-40% for optimization work
- **Security Sensitive**: Add 40-80% for security-related changes

### Estimation Best Practices
- **Use Realistic Estimates**: Default to realistic range for planning
- **Account for Unknowns**: Use pessimistic range for critical deadlines
- **Break Down Large Tasks**: Use `--detailed` for complex changes
- **Include Testing Time**: Use `--include-tests` for comprehensive estimates
- **Review and Adjust**: Calibrate estimates based on actual vs. estimated time

## Integration with Development Workflow

### Pre-Development
- Use before starting new features or major changes
- Estimate effort for sprint planning and resource allocation
- Identify potentially complex changes early

### During Development
- Re-estimate if scope changes significantly
- Use detailed breakdown for complex implementations
- Track actual vs. estimated time for calibration

### Post-Development
- Compare actual time with estimates for learning
- Update experience profile based on performance
- Refine estimation models based on historical data

## Troubleshooting

### Common Issues
- **No Files Detected**: Check git status and file paths
- **Inaccurate Estimates**: Review experience profile configuration
- **Missing Technologies**: Add new technologies to experience profile
- **Complexity Misclassification**: Use `--complexity` flag to override

### Calibration Tips
- **Track Actual Time**: Compare estimates with actual development time
- **Adjust Multipliers**: Modify experience multipliers based on performance
- **Update Experience Levels**: Keep experience profile current
- **Consider Project Factors**: Account for project-specific complexity

## Success Criteria

### Estimation Quality
- **Accuracy**: Estimates within 20% of actual time
- **Consistency**: Similar changes produce similar estimates
- **Calibration**: Estimates improve over time with feedback
- **Actionability**: Estimates enable effective planning and resource allocation

### Command Effectiveness
- **Speed**: Analysis completes in under 30 seconds
- **Accuracy**: Correctly identifies change types and complexity
- **Flexibility**: Handles various file types and project structures
- **Integration**: Works seamlessly with existing development workflow

**Remember**: The `/estimate-dev-time` command is a planning tool that provides data-driven estimates based on your experience profile and code analysis. Use it to inform planning decisions, but always consider project-specific factors and adjust estimates based on your judgment and historical performance.
