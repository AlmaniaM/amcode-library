# Test Planning Command

## Purpose
Comprehensive test planning, test strategy development, and testing specification creation with persistent documentation. This command ensures thorough test planning before any testing implementation begins, covering unit tests, integration tests, E2E tests, and performance testing.

**Output Location**: All test planning documents are saved in the `test-planning/` folder for organized tracking and easy access.

## When to Use
- **New Feature Testing**: Planning comprehensive test coverage for new features or components
- **Bug Fix Testing**: Planning regression testing and verification for bug fixes
- **Test Suite Expansion**: Planning additional test coverage for existing functionality
- **E2E Test Planning**: Planning end-to-end test scenarios and user workflows
- **Performance Testing**: Planning performance, load, and stress testing strategies
- **Test Refactoring**: Planning test code improvements and optimization
- **Test Migration**: Planning migration of tests between frameworks or versions
- **Test Infrastructure**: Planning test environment setup and CI/CD integration

## Command Structure

### Phase 1: Test Planning Initialization & Document Management
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive test planning setup and document management to ensure persistent, detailed test planning documentation.

#### Test Planning Document Management
- **Document Identification**: Determine if working on existing test planning document or creating new one
- **Document Location**: Use `test-planning/` directory for test planning documents
- **Folder Creation**: Ensure `test-planning/` directory exists before creating documents
- **Document Naming**: Use descriptive names like `feature-name-test-planning.md` or `bug-fix-test-planning.md`
- **Document Structure**: Follow consistent test planning document template
- **Version Control**: Track test planning iterations and updates

#### Test Scope Assessment
- **Test Coverage Scope**: Define what needs to be tested and to what level
- **Test Type Scope**: Identify unit, integration, E2E, performance, and accessibility testing needs
- **Component Scope**: Identify components, services, and modules requiring test coverage
- **User Journey Scope**: Define critical user paths and workflows to test
- **Risk Scope**: Identify testing risks and mitigation strategies

### Phase 2: Comprehensive Test Planning Execution
**Technology-Adaptive Protocols**: Apply test planning methods appropriate to the type of testing being planned.

#### Unit Test Planning
- **Component Analysis**: Identify components, services, and utilities requiring unit tests
- **Test Case Design**: Plan test cases for each method, function, and component
- **Mock Strategy**: Plan mocking approach for dependencies and external services
- **Test Data Management**: Plan test data setup, fixtures, and cleanup strategies
- **Coverage Goals**: Set code coverage targets and identify critical paths
- **Test Organization**: Plan test file structure and naming conventions

#### Integration Test Planning
- **API Testing**: Plan API endpoint testing and integration scenarios
- **Service Integration**: Plan testing of service interactions and data flow
- **Database Testing**: Plan database integration and data persistence testing
- **External Service Testing**: Plan testing of third-party service integrations
- **State Management Testing**: Plan testing of application state and data flow
- **Error Handling Testing**: Plan testing of error scenarios and edge cases

#### E2E Test Planning
- **User Journey Mapping**: Identify critical user workflows and scenarios
- **Test Environment Setup**: Plan test environment configuration and data setup
- **Test Data Strategy**: Plan test data creation, management, and cleanup
- **Cross-Browser Testing**: Plan browser compatibility and responsive testing
- **Performance Testing**: Plan performance and load testing scenarios
- **Accessibility Testing**: Plan accessibility testing and WCAG compliance verification

#### Performance Test Planning
- **Performance Metrics**: Define performance benchmarks and success criteria
- **Load Testing Scenarios**: Plan load testing with various user loads
- **Stress Testing**: Plan stress testing to identify breaking points
- **Memory Testing**: Plan memory leak detection and optimization testing
- **Bundle Size Testing**: Plan bundle size monitoring and optimization testing
- **Core Web Vitals**: Plan Core Web Vitals testing and optimization

### Phase 3: Detailed Test Planning Documentation
**Universal Frontend Stewardship**: Create comprehensive test planning documentation that serves as implementation guide.

#### Test Planning Document Template
```markdown
# [Feature/Component/System] Test Planning Document

## Test Planning Summary
- **Planning Date**: [Date]
- **Planning Duration**: [Time spent]
- **Planning Status**: [In Progress/Complete/Blocked]
- **Next Review Date**: [Date]
- **Test Framework**: [Jest, Playwright, Cypress, etc.]

## Executive Summary
- **Testing Objective**: [What we're trying to test]
- **Success Criteria**: [How we measure testing success]
- **Key Test Deliverables**: [What tests will be delivered]
- **Timeline**: [Estimated test completion date]

## Test Requirements Analysis
### Functional Test Requirements
- [ ] [Test requirement 1]
- [ ] [Test requirement 2]
- [ ] [Test requirement 3]

### Non-Functional Test Requirements
- [ ] [Performance test requirements]
- [ ] [Security test requirements]
- [ ] [Accessibility test requirements]
- [ ] [Browser compatibility test requirements]

## Test Architecture
### Test Framework Overview
[High-level test framework and tooling description]

### Test Structure Design
- **Unit Tests**: [Structure and organization]
- **Integration Tests**: [Structure and organization]
- **E2E Tests**: [Structure and organization]
- **Performance Tests**: [Structure and organization]

### Test Data Management
[Description of test data strategy and management]

### Test Environment Configuration
- **Development Environment**: [Configuration details]
- **CI/CD Environment**: [Configuration details]
- **Staging Environment**: [Configuration details]

## Test Implementation Plan
### Phase 1: [Phase Name]
- [ ] [Test task 1]
- [ ] [Test task 2]
- [ ] [Test task 3]

### Phase 2: [Phase Name]
- [ ] [Test task 1]
- [ ] [Test task 2]
- [ ] [Test task 3]

## Test Dependencies & Prerequisites
### External Dependencies
- [ ] [Test framework dependency 1]
- [ ] [Test data dependency 2]

### Internal Dependencies
- [ ] [Component dependency 1]
- [ ] [Service dependency 2]

## Test Risk Assessment
### High Risk
- [ ] [Test risk 1]: [Mitigation strategy]
- [ ] [Test risk 2]: [Mitigation strategy]

### Medium Risk
- [ ] [Test risk 1]: [Mitigation strategy]
- [ ] [Test risk 2]: [Mitigation strategy]

## Test Strategy by Type
### Unit Testing Strategy
- [ ] [Unit test case 1]
- [ ] [Unit test case 2]

### Integration Testing Strategy
- [ ] [Integration test case 1]
- [ ] [Integration test case 2]

### E2E Testing Strategy
- [ ] [E2E test case 1]
- [ ] [E2E test case 2]

### Performance Testing Strategy
- [ ] [Performance test case 1]
- [ ] [Performance test case 2]

## Test Coverage Analysis
### Code Coverage Targets
- **Unit Tests**: [Target percentage]
- **Integration Tests**: [Target percentage]
- **E2E Tests**: [Target percentage]

### Critical Path Coverage
- [ ] [Critical path 1]
- [ ] [Critical path 2]

### Edge Case Coverage
- [ ] [Edge case 1]
- [ ] [Edge case 2]

## Test Data Strategy
### Test Data Requirements
- [ ] [Test data requirement 1]
- [ ] [Test data requirement 2]

### Test Data Management
- [ ] [Data setup strategy]
- [ ] [Data cleanup strategy]

## Test Environment Setup
### Development Environment
- [ ] [Environment setup task 1]
- [ ] [Environment setup task 2]

### CI/CD Integration
- [ ] [CI/CD setup task 1]
- [ ] [CI/CD setup task 2]

## Outstanding Questions
### Critical Questions
- [ ] [Question 1]: [Why this is critical]
- [ ] [Question 2]: [Why this is critical]

### Important Questions
- [ ] [Question 1]: [Why this is important]
- [ ] [Question 2]: [Why this is important]

### Nice-to-Have Questions
- [ ] [Question 1]: [Why this is nice to have]
- [ ] [Question 2]: [Why this is nice to have]

## Next Steps
1. [Next step 1]
2. [Next step 2]
3. [Next step 3]

## Test Planning History
- **[Date]**: [What was planned/updated]
- **[Date]**: [What was planned/updated]
```

### Phase 4: Test Planning Validation & Quality Assurance
**Multi-Layer Verification**: Ensure comprehensive test planning and clear documentation.

#### Test Planning Quality Gates
- **Completeness Check**: Verify all important testing aspects are planned
- **Technical Feasibility**: Confirm planned testing approach is technically sound
- **Coverage Assessment**: Ensure comprehensive test coverage is planned
- **Risk Assessment**: Ensure all major testing risks are identified
- **Dependency Mapping**: Confirm all test dependencies are identified
- **Framework Compatibility**: Verify test framework choices are appropriate

#### Test Planning Review Process
- **Self-Review**: Review test planning document for completeness and accuracy
- **Stakeholder Review**: Identify stakeholders who should review test planning
- **Technical Review**: Ensure technical testing approach is sound
- **Quality Assurance Review**: Verify testing strategy meets quality standards
- **Implementation Readiness**: Confirm test planning is ready for implementation

## Universal Test Planning Protocols

### Technology-Adaptive Test Planning
- **Framework Detection**: Automatically identify and adapt to different testing frameworks
- **Language-Specific Patterns**: Apply test planning techniques appropriate to the programming language
- **Architecture Recognition**: Identify and plan testing for different architectural patterns
- **Best Practice Application**: Apply relevant testing best practices and conventions

### Test Complexity Handling
- **Simple Tests**: Quick, focused test planning with key test cases
- **Complex Tests**: Layered test planning with progressive detail
- **Large Test Suites**: Structured breakdown with clear organization
- **Ambiguous Requirements**: Multiple testing approaches with clarification questions

### Test Planning Quality Gates
- **Clarity**: Test planning is clear and unambiguous
- **Completeness**: All important testing aspects are covered
- **Accuracy**: Test planning correctly represents testing needs
- **Actionability**: Test planning leads to clear implementation steps
- **Contextual Relevance**: Test planning is relevant to the specific testing needs

## Usage Examples

### Creating Test Planning Documents
```bash
# Create test planning folder and document
mkdir -p test-planning
TEST_PLANNING_DOC="test-planning/feature-name-test-planning.md"
echo "# Feature Name Test Planning Document" > "$TEST_PLANNING_DOC"

# Add test planning sections
echo "## Test Planning Summary" >> "$TEST_PLANNING_DOC"
echo "## Executive Summary" >> "$TEST_PLANNING_DOC"
echo "## Test Requirements Analysis" >> "$TEST_PLANNING_DOC"
echo "## Test Architecture" >> "$TEST_PLANNING_DOC"
```

### Test Planning Commands
- `/test-planning` - Execute comprehensive test planning for current task
- `/test-planning --unit` - Focus on unit test planning
- `/test-planning --integration` - Focus on integration test planning
- `/test-planning --e2e` - Focus on E2E test planning
- `/test-planning --performance` - Focus on performance test planning
- `/test-planning @src/components/` - Plan tests for specific component directory
- `/test-planning --validation` - Focus on test planning validation and review

### Feature Test Planning
```
/test-planning
Plan comprehensive testing for the new user authentication feature including unit tests, integration tests, and E2E tests.
```

### Bug Fix Test Planning
```
/test-planning
Plan regression testing and verification for the login bug fix, including edge cases and error scenarios.
```

### E2E Test Planning
```
/test-planning
Plan end-to-end testing for the complete user registration and onboarding workflow.
```

### Performance Test Planning
```
/test-planning
Plan performance testing strategy for the dashboard component including load testing and Core Web Vitals monitoring.
```

## Success Criteria

### Test Planning Quality
- **Comprehensive**: All important testing aspects are planned
- **Accurate**: Test planning correctly represents testing needs
- **Clear**: Communication is clear and easy to follow
- **Actionable**: Test planning leads to clear implementation steps
- **Contextual**: Test planning is relevant to the specific testing needs

### Test Implementation Readiness
- **Framework Selection**: Appropriate testing frameworks are selected
- **Coverage Planning**: Comprehensive test coverage is planned
- **Environment Setup**: Test environment requirements are defined
- **Data Strategy**: Test data management is planned
- **CI/CD Integration**: Continuous testing integration is planned

## Integration with Other Commands

### Pre-Implementation
- Use before `/do` to ensure complete test planning before implementation
- Use before `/feature-implement` to plan testing for new features
- Use before `/refactor` to plan testing for refactored code

### Test Implementation Support
- Use with `/test-implement` for comprehensive test implementation
- Use with `/test-debug` to understand test failures before fixing
- Use with `/test-optimize` to plan test performance improvements

### Quality Assurance
- Use for planning comprehensive test coverage
- Use for planning test automation and CI/CD integration
- Use for planning test maintenance and optimization

**Remember**: The `/test-planning` command is your gateway to comprehensive test strategy. Use it whenever you need to plan testing for any feature, bug fix, or system component. The command adapts to your specific testing needs and provides the level of test planning appropriate for your context.
