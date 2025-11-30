# Deep Code Analysis Protocol

## Overview

Execute comprehensive analysis of specific code files, classes, or components using systematic code reconnaissance and dependency mapping. This command applies the Senior Frontend Architect doctrine for thorough understanding of code-level concerns in frontend monorepo projects.

## Mission Briefing: Deep Code Analysis Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-general`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute a comprehensive analysis of specific code targets using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows the Phase 0 reconnaissance principles with enhanced depth for complete code understanding. The goal is to map and understand the code structure and patterns, not to provide recommendations or improvements.

---

## **Phase 0: Systematic Code Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform exhaustive, non-destructive analysis of the specified code targets to build a complete, evidence-based mental model of the code structure, dependencies, and patterns.

**Code Analysis Scope:**

- **Target Code Structure Analysis**: Complete understanding of the specific code file or class
- **Dependency Chain Analysis**: Deep analysis of all dependencies and their implementations
- **Usage Context Analysis**: How the target code fits into the broader system
- **Pattern Analysis**: Code patterns, architectural decisions, and design principles
- **Business Logic Analysis**: Understanding of code intention and business logic
- **Performance Analysis**: Code-level performance patterns and optimizations
- **Accessibility Analysis**: Code-level accessibility patterns and compliance
- **Documentation Analysis**: README.md files and code documentation for context understanding

**Constraints:**

- **No mutations are permitted during this phase**
- **No recommendations or improvement suggestions should be provided**
- **Focus solely on understanding and mapping the existing code**

---

## **Phase 1: Target Code Structure Deep Analysis**

**Directive:** Comprehensive analysis of the target code's structure and organization.

**Analysis Areas:**

1. **Code File Structure Analysis**:
   - File organization and naming conventions
   - Import/export patterns and module structure
   - Class/interface organization and hierarchy
   - Method and property organization

2. **Component Structure Analysis**:
   - Component decorator configuration and metadata
   - Input/Output property definitions and interfaces
   - Lifecycle hook implementation and patterns
   - Template and styling integration

3. **Service Structure Analysis**:
   - Service decorator and dependency injection patterns
   - Method organization and responsibility separation
   - State management and data flow patterns
   - Error handling and validation patterns

4. **TypeScript Structure Analysis**:
   - Type definitions and interface implementations
   - Generic type usage and constraints
   - Type safety patterns and strict typing
   - Enum and union type usage

5. **Code Organization Patterns**:
   - Single Responsibility Principle adherence
   - DRY (Don't Repeat Yourself) principle implementation
   - Code modularity and reusability patterns
   - Clean code principles and best practices

**Output:** Complete target code structure documentation with architectural patterns and organization.

---

## **Phase 2: Dependency Chain Deep Analysis**

**Directive:** Deep analysis of all dependencies and their complete implementation chains.

**Analysis Areas:**

1. **Direct Dependencies Analysis**:
   - All import statements and their purposes
   - External library dependencies and versions
   - Internal module dependencies and relationships
   - Type-only vs runtime dependencies

2. **Transitive Dependencies Analysis**:
   - Dependencies of dependencies (second-level analysis)
   - Critical dependency chains and their impact
   - Circular dependency detection and analysis
   - Dependency version compatibility analysis

3. **Dependency Implementation Analysis**:
   - Read and analyze each dependency's implementation
   - Understand what each dependency provides
   - Map dependency interfaces and contracts
   - Analyze dependency performance implications

4. **Dependency Usage Patterns**:
   - How dependencies are used within the target code
   - Dependency injection patterns and strategies
   - Service layer dependencies and data flow
   - Component dependencies and composition patterns

5. **Dependency Risk Analysis**:
   - Outdated or deprecated dependencies
   - Security vulnerabilities in dependencies
   - Performance implications of dependencies
   - Maintenance and update considerations

**Output:** Complete dependency chain documentation with implementation details and risk assessment.

---

## **Phase 3: Usage Context Deep Analysis**

**Directive:** Analyze how the target code fits into the broader system and is used by other code.

**Analysis Areas:**

1. **Consumer Code Analysis**:
   - Find all code that imports or uses the target code
   - Analyze how the target code is consumed
   - Map usage patterns and integration points
   - Understand consumer expectations and contracts

2. **Integration Point Analysis**:
   - API surface and public interface analysis
   - Input/output validation and error handling
   - Event handling and communication patterns
   - State management and data flow integration

3. **System Integration Analysis**:
   - How the target code integrates with the broader system
   - Data flow and state management integration
   - Service layer integration and communication
   - Component hierarchy and composition patterns

4. **Dependency Impact Analysis**:
   - Impact of changes to the target code on consumers
   - Breaking change risk assessment
   - Backward compatibility considerations
   - Migration and refactoring implications

5. **Performance Impact Analysis**:
   - Performance implications of the target code
   - Bundle size impact and optimization opportunities
   - Runtime performance characteristics
   - Memory usage and optimization patterns

6. **Documentation Context Analysis**:
   - README.md files and project documentation for context
   - Code comments and inline documentation analysis
   - API documentation and usage examples
   - Development guidelines and best practices

**Output:** Complete usage context documentation with integration patterns and impact analysis.

---

## **Phase 4: Code Pattern Deep Analysis**

**Directive:** Comprehensive analysis of code patterns, architectural decisions, and design principles.

**Analysis Areas:**

1. **Framework Pattern Analysis**:
   - Component lifecycle patterns and best practices
   - Service injection and dependency management patterns
   - Reactive programming patterns and state management
   - Data flow and state management patterns

2. **TypeScript Pattern Analysis**:
   - Type safety patterns and strict typing usage
   - Generic type patterns and constraints
   - Interface design and contract patterns
   - Error handling and validation patterns

3. **Design Pattern Analysis**:
   - Creational patterns (Factory, Builder, Singleton)
   - Structural patterns (Adapter, Decorator, Facade)
   - Behavioral patterns (Observer, Strategy, Command)
   - Framework-specific patterns (Component, Service, Guard, etc.)

4. **Architectural Pattern Analysis**:
   - Clean Architecture principles and implementation
   - SOLID principles adherence and implementation
   - Separation of concerns and modularity
   - Testability and maintainability patterns

5. **Performance Pattern Analysis**:
   - Memoization and caching patterns
   - Lazy loading and code splitting patterns
   - Change detection optimization patterns
   - Bundle optimization and tree shaking patterns

6. **Error Handling Pattern Analysis**:
   - Error boundary patterns and implementation
   - Exception handling and recovery patterns
   - Logging and monitoring patterns
   - User feedback and error communication patterns

**Output:** Complete code pattern documentation with architectural decisions and design principles.

---

## **Phase 5: Business Logic Deep Analysis**

**Directive:** Analyze the business logic and code intention without requiring domain knowledge.

**Analysis Areas:**

1. **Code Intent Analysis**:
   - Understand what the code is trying to accomplish
   - Map business rules and logic flows
   - Identify decision points and conditional logic
   - Analyze data transformations and calculations

2. **Algorithm Analysis**:
   - Identify algorithms and their complexity
   - Analyze data structures and their usage
   - Map computational patterns and optimizations
   - Understand sorting, searching, and filtering logic

3. **Data Flow Analysis**:
   - Map data input sources and validation
   - Trace data transformations and processing
   - Identify output destinations and formatting
   - Analyze data persistence and state management

4. **Business Rule Analysis**:
   - Identify business rules and constraints
   - Map validation logic and error conditions
   - Analyze business process flows
   - Understand decision trees and branching logic

5. **Integration Logic Analysis**:
   - Map external system integration patterns
   - Analyze API communication and data exchange
   - Understand event handling and messaging
   - Map workflow and process orchestration

6. **Security Logic Analysis**:
   - Identify authentication and authorization patterns
   - Analyze input validation and sanitization
   - Map security constraints and access control
   - Understand data protection and privacy patterns

**Output:** Complete business logic documentation with code intention and algorithmic patterns.

---

## **Phase 6: Performance Deep Analysis**

**Directive:** Comprehensive analysis of code-level performance patterns and optimizations.

**Analysis Areas:**

1. **Runtime Performance Analysis**:
   - Method execution time and complexity analysis
   - Memory usage patterns and optimization opportunities
   - Change detection impact and optimization
   - Event handling and callback performance

2. **Bundle Performance Analysis**:
   - Code splitting and lazy loading implementation
   - Tree shaking and dead code elimination
   - Import optimization and dependency analysis
   - Bundle size impact and optimization opportunities

3. **Framework Performance Analysis**:
   - Change detection strategy usage
   - List rendering optimization patterns
   - Async pattern usage and subscription management
   - Component lifecycle optimization

4. **Memory Management Analysis**:
   - Memory leak detection and prevention patterns
   - Subscription management and cleanup
   - Event listener management and cleanup
   - Object lifecycle and garbage collection optimization

5. **Network Performance Analysis**:
   - HTTP request optimization and caching
   - Data fetching patterns and efficiency
   - API call optimization and batching
   - Error handling and retry patterns

**Output:** Complete performance analysis documentation with optimization opportunities and patterns.

---

## **Phase 7: Accessibility Deep Analysis**

**Directive:** Comprehensive analysis of code-level accessibility patterns and WCAG compliance.

**Analysis Areas:**

1. **Semantic HTML Analysis**:
   - HTML structure and semantic element usage
   - ARIA attribute implementation and correctness
   - Heading hierarchy and document structure
   - Form accessibility and label associations

2. **Keyboard Navigation Analysis**:
   - Tab order and focus management
   - Keyboard event handling and accessibility
   - Focus indicators and visual feedback
   - Skip links and navigation shortcuts

3. **Screen Reader Analysis**:
   - Alternative text and descriptive content
   - Live regions and dynamic content updates
   - Form labels and error message associations
   - Content structure and navigation patterns

4. **Color and Contrast Analysis**:
   - Color contrast compliance and validation
   - Color-independent information conveyance
   - High contrast mode support
   - Visual indicator accessibility

5. **Interactive Element Analysis**:
   - Button and link accessibility
   - Form control accessibility and validation
   - Modal and dialog accessibility
   - Custom component accessibility patterns

**Output:** Complete accessibility analysis documentation with WCAG compliance patterns and recommendations.

---

## **Phase 8: Comprehensive Code Architecture Map**

**Directive:** Synthesize all code-level analysis into a comprehensive understanding.

**Output Requirements:**

1. **Code Structure Summary**: High-level overview of the target code architecture
2. **Dependency Map**: Complete dependency chain with implementation details
3. **Usage Context Map**: How the code fits into the broader system
4. **Pattern Analysis**: Code patterns and architectural decisions
5. **Business Logic Map**: Code intention and business rule implementation
6. **Performance Analysis**: Code-level performance characteristics and optimizations
7. **Accessibility Analysis**: Accessibility patterns and WCAG compliance
8. **Key Patterns**: Critical code patterns and their implementation details

**Format:** Structured, searchable documentation with clear categorization and cross-references.

---

## **Phase 9: Comprehensive Code Understanding Map**

**Directive:** Synthesize all code-level analysis into a complete understanding of the target code.

**Understanding Requirements:**

- **Code Structure Map**: Complete code organization and architectural patterns
- **Dependency Map**: Complete dependency chain with implementation details
- **Usage Context Map**: How the code integrates with the broader system
- **Pattern Map**: Code patterns, architectural decisions, and design principles
- **Business Logic Map**: Code intention and business rule implementation
- **Performance Map**: Code-level performance characteristics and patterns
- **Accessibility Map**: Accessibility patterns and WCAG compliance implementation
- **Security Map**: Security patterns and implementation details

**Format:** Structured, searchable documentation with clear categorization and cross-references.

## Usage Examples

- `/analyze-code Analyze the UserService class and understand its dependencies, usage patterns, and business logic`
- `/analyze-code Deep dive into the UserProfileComponent and analyze its structure, dependencies, and integration patterns`
- `/analyze-code Analyze the AuthGuard and understand its security patterns, dependencies, and usage context`
- `/analyze-code Comprehensive analysis of the DataService and its API integration patterns`
- `/analyze-code Analyze the CustomPipe and understand its transformation logic and performance implications`
- `/analyze-code Review the ErrorHandlerService and analyze its error handling patterns and integration`
- `/analyze-code Analyze the ThemeService and understand its styling patterns and configuration management`
