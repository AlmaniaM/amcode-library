# Pull Request Review Command

## Overview

Comprehensive pull request review that analyzes changed files against the target branch, reviewing based on linting rules and the established LLM profile. This command provides systematic code review with focus on code quality, architecture, and adherence to project standards.

## Mission Briefing: Pull Request Review Protocol

**PR REVIEW COMMAND:** Execute comprehensive pull request analysis using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This command reviews code changes against established standards, linting rules, and architectural patterns.

---

## **Phase 0: PR Analysis & Change Detection (Read-Only)**

**Directive:** Analyze the pull request changes and identify all modified files, additions, and deletions.

**Change Analysis Scope:**

- **File Change Detection**: Identify all modified, added, and deleted files
- **Diff Analysis**: Understand the nature and scope of changes
- **Impact Assessment**: Evaluate the impact on existing codebase
- **Context Understanding**: Understand the purpose and context of changes

**Constraints:**

- **No mutations are permitted during this phase**
- **Focus on understanding changes, not implementing fixes**
- **Maintain objective analysis perspective**

---

## **Phase 1: Linting Rule Analysis**

**Directive:** Analyze all changed files against the project's linting rules and identify violations.

**Linting Analysis Process:**

1. **ESLint Rule Validation**:
   ```bash
   # Run ESLint on changed files
   npx eslint [changed-files] --format=json
   
   # Check specific rule violations
   npx eslint [changed-files] --format=json --rule="rule-name:error"
   ```

2. **Rule Violation Classification**:
   - **ERROR Level**: Critical issues that must be fixed
   - **WARNING Level**: Issues that should be addressed
   - **INFO Level**: Suggestions for improvement
   - **Custom Rules**: Project-specific rule violations

3. **Angular-Specific Rule Analysis**:
   - Component lifecycle compliance
   - Template syntax validation
   - Service injection patterns
   - Module boundary adherence
   - TypeScript type safety

4. **Code Style Analysis**:
   - Prettier formatting compliance
   - Import/export organization
   - Naming convention adherence
   - Code structure patterns

---

## **Phase 2: Architecture & Design Review**

**Directive:** Review changes against established architectural patterns and design principles.

**Architecture Review Areas:**

1. **Monorepo Structure Compliance**:
   - Nx workspace boundary adherence
   - Library dependency management
   - Module organization patterns
   - Shared code utilization

2. **Angular Architecture Patterns**:
   - Component design and responsibility
   - Service architecture and injection
   - State management patterns (NgRx)
   - Routing and navigation structure
   - Lazy loading implementation

3. **Code Organization**:
   - File structure and naming
   - Import/export patterns
   - Interface and type definitions
   - Utility function placement

4. **Design Pattern Adherence**:
   - Clean Code principles
   - SOLID principles
   - DRY (Don't Repeat Yourself)
   - Separation of Concerns

---

## **Phase 3: Code Quality Assessment**

**Directive:** Evaluate code quality, maintainability, and best practices adherence.

**Quality Assessment Categories:**

1. **Code Readability**:
   - Variable and function naming
   - Code structure and organization
   - Comment quality and necessity
   - Documentation completeness

2. **Maintainability**:
   - Code complexity analysis
   - Function length and responsibility
   - Class design and cohesion
   - Dependency management

3. **Performance Considerations**:
   - Bundle size impact
   - Change detection optimization
   - Memory leak prevention
   - Lazy loading implementation

4. **Security Analysis**:
   - Input validation patterns
   - XSS prevention measures
   - CSRF protection
   - Authentication and authorization

---

## **Phase 4: Testing & Verification Review**

**Directive:** Assess testing coverage and quality for the changes.

**Testing Review Areas:**

1. **Unit Test Coverage**:
   - Test file existence for new components
   - Test coverage for modified functions
   - Test quality and completeness
   - Mock usage and setup

2. **Integration Test Review**:
   - Component integration testing
   - Service integration testing
   - API integration testing
   - End-to-end test updates

3. **Test Quality Assessment**:
   - Test naming and organization
   - Test data setup and teardown
   - Assertion quality and coverage
   - Test maintainability

---

## **Phase 5: Documentation & Communication Review**

**Directive:** Evaluate documentation updates and communication clarity.

**Documentation Review Areas:**

1. **Code Documentation**:
   - JSDoc comments for new functions
   - Interface and type documentation
   - Component usage documentation
   - API documentation updates

2. **README and Documentation**:
   - Feature documentation updates
   - Setup and installation instructions
   - Configuration changes documentation
   - Breaking changes documentation

3. **Commit Message Quality**:
   - Commit message clarity and detail
   - Conventional commit format adherence
   - Change description accuracy
   - Issue reference inclusion

---

## **Phase 6: Risk Assessment & Impact Analysis**

**Directive:** Evaluate potential risks and impacts of the changes.

**Risk Assessment Categories:**

1. **Breaking Changes**:
   - API contract changes
   - Public interface modifications
   - Configuration changes
   - Dependency updates

2. **Performance Impact**:
   - Bundle size changes
   - Runtime performance impact
   - Memory usage changes
   - Network request changes

3. **Security Implications**:
   - New security vulnerabilities
   - Authentication changes
   - Data handling modifications
   - Third-party dependency updates

4. **Maintenance Impact**:
   - Code complexity changes
   - Technical debt introduction
   - Future maintenance burden
   - Team knowledge requirements

---

## **Phase 7: Comprehensive Review Report**

**Directive:** Generate a comprehensive review report with actionable feedback.

**Report Structure:**

### **Executive Summary**
- **Change Overview**: Summary of all changes made
- **Overall Assessment**: High-level quality assessment
- **Critical Issues**: Must-fix issues identified
- **Recommendations**: Key improvement suggestions

### **Linting Analysis Results**
- **Error Count**: Number of linting errors found
- **Warning Count**: Number of linting warnings found
- **Rule Violations**: Specific rule violations by category
- **Fix Recommendations**: Suggested fixes for each violation

### **Architecture Review Results**
- **Pattern Compliance**: Adherence to established patterns
- **Design Quality**: Assessment of design decisions
- **Structure Issues**: Organizational problems identified
- **Improvement Suggestions**: Architectural improvements recommended

### **Code Quality Assessment**
- **Readability Score**: Code readability assessment
- **Maintainability Score**: Code maintainability assessment
- **Performance Impact**: Performance implications
- **Security Review**: Security considerations

### **Testing Review Results**
- **Coverage Analysis**: Test coverage assessment
- **Test Quality**: Quality of existing tests
- **Missing Tests**: Tests that should be added
- **Test Improvements**: Suggestions for test enhancement

### **Documentation Review**
- **Documentation Completeness**: Assessment of documentation
- **Missing Documentation**: Documentation gaps identified
- **Quality Issues**: Documentation quality problems
- **Improvement Suggestions**: Documentation enhancement recommendations

### **Risk Assessment**
- **High Risk Issues**: Critical issues requiring attention
- **Medium Risk Issues**: Issues that should be addressed
- **Low Risk Issues**: Minor issues for future consideration
- **Mitigation Strategies**: Recommended risk mitigation approaches

### **Action Items**
- **Must Fix**: Critical issues that must be resolved
- **Should Fix**: Important issues that should be addressed
- **Could Fix**: Nice-to-have improvements
- **Future Considerations**: Items for future development

---

## **Usage Examples**

- `/pr-review` - Review current PR changes against main branch
- `/pr-review --target=develop` - Review against develop branch
- `/pr-review @src/components/` - Review specific directory changes
- `/pr-review --focus=linting` - Focus on linting issues only
- `/pr-review --focus=architecture` - Focus on architectural review
- `/pr-review --focus=testing` - Focus on testing review

## **Review Focus Areas**

### **Linting Focus**
- ESLint rule violations
- TypeScript errors and warnings
- Angular-specific rule compliance
- Code style and formatting issues

### **Architecture Focus**
- Monorepo structure compliance
- Angular architecture patterns
- Design pattern adherence
- Code organization and structure

### **Quality Focus**
- Code readability and maintainability
- Performance implications
- Security considerations
- Best practices adherence

### **Testing Focus**
- Test coverage and quality
- Test organization and structure
- Mock usage and setup
- Integration testing considerations

## **Integration with Project Standards**

- **ESLint Configuration**: Uses project's linting rules
- **Angular Standards**: Follows Angular best practices
- **Nx Workspace**: Respects monorepo structure and boundaries
- **TypeScript Standards**: Enforces TypeScript best practices
- **Testing Standards**: Follows project testing patterns

## **Review Quality Assurance**

- **Systematic Analysis**: Comprehensive review of all aspects
- **Objective Assessment**: Unbiased evaluation of code quality
- **Actionable Feedback**: Specific, implementable recommendations
- **Risk Identification**: Proactive identification of potential issues
- **Best Practice Guidance**: Recommendations based on established patterns

**Remember**: This command provides comprehensive, systematic review of pull request changes, focusing on code quality, architecture compliance, and adherence to project standards while maintaining the established LLM profile and professional communication standards.
