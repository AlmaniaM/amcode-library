# Shallow Code Analysis Protocol

## Overview

Execute focused analysis of specific code files, classes, or components using streamlined reconnaissance and essential pattern detection. This command provides practical code insights while minimizing context usage, focusing on the most critical code patterns and functionality.

## Mission Briefing: Shallow Code Analysis Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-general`, `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute a focused analysis of specific code targets using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows streamlined reconnaissance principles for essential code understanding. The goal is to map key code structure and patterns efficiently.

---

## **Phase 0: Essential Code Reconnaissance (Read-Only)**

**Directive:** Perform focused analysis of critical code-level concerns to build an essential understanding of the code structure and patterns.

**Essential Analysis Scope:**

- **Code Structure**: File organization, class/module structure, key methods
- **Dependencies**: Essential imports, external dependencies, key relationships
- **Patterns**: Critical design patterns, framework patterns, architectural decisions
- **Functionality**: Core business logic, key algorithms, essential behavior
- **Quality**: Basic code quality indicators and potential issues
- **Documentation**: README.md files and essential code documentation

**Constraints:**

- **No mutations are permitted during this phase**
- **Focus on essential patterns and functionality only**
- **Minimize detailed analysis in favor of practical insights**

---

## **Phase 1: Code Structure Analysis**

**Directive:** Identify and analyze the essential code structure and organization.

**Essential Structure Areas:**

1. **File Organization**:
   - File type and purpose (component, service, utility)
   - Key classes, interfaces, and methods
   - Import/export patterns

2. **Framework Integration**:
   - Angular component patterns (if applicable)
   - TypeScript patterns and conventions
   - Service injection and dependency patterns

3. **Code Organization**:
   - Method organization and responsibility
   - Class hierarchy and relationships
   - Key data structures and types

**Output:** Essential code structure map with key components.

---

## **Phase 2: Dependency Analysis**

**Directive:** Map essential dependencies and relationships.

**Essential Dependency Areas:**

1. **External Dependencies**:
   - Key external libraries and frameworks
   - Critical API dependencies
   - Essential utility libraries

2. **Internal Dependencies**:
   - Key internal module dependencies
   - Service dependencies and relationships
   - Component dependencies and composition

3. **Configuration Dependencies**:
   - Environment configuration dependencies
   - Build tool dependencies
   - Runtime configuration requirements

4. **Documentation Dependencies**:
   - README.md files for project context
   - Code comments and inline documentation
   - Essential development guidelines

**Output:** Essential dependency map with key relationships.

---

## **Phase 3: Pattern Analysis**

**Directive:** Identify critical code patterns and architectural decisions.

**Essential Pattern Categories:**

1. **Design Patterns**:
   - Creational patterns (Factory, Builder, Singleton)
   - Structural patterns (Adapter, Decorator, Facade)
   - Behavioral patterns (Observer, Strategy, Command)

2. **Framework Patterns**:
   - Angular patterns (components, services, guards)
   - TypeScript patterns (types, interfaces, generics)
   - RxJS patterns (observables, operators, subscriptions)

3. **Architectural Patterns**:
   - Clean Architecture principles
   - SOLID principles implementation
   - Error handling and validation patterns

**Output:** Essential pattern inventory with key implementations.

---

## **Phase 4: Functionality Analysis**

**Directive:** Understand essential code functionality and business logic.

**Essential Functionality Areas:**

1. **Core Functionality**:
   - Primary purpose and responsibilities
   - Key algorithms and data processing
   - Essential business logic

2. **Data Flow**:
   - Input sources and validation
   - Data transformation and processing
   - Output generation and formatting

3. **Integration Points**:
   - API interactions and data exchange
   - Event handling and communication
   - State management and persistence

**Output:** Essential functionality map with key behaviors.

---

## **Phase 5: Quality Assessment**

**Directive:** Assess essential code quality and identify key issues.

**Essential Quality Areas:**

1. **Code Quality**:
   - Readability and maintainability
   - Code organization and structure
   - Naming conventions and clarity

2. **Performance**:
   - Key performance characteristics
   - Potential bottlenecks and optimizations
   - Memory usage and efficiency

3. **Security**:
   - Input validation and sanitization
   - Authentication and authorization
   - Data protection and privacy

**Output:** Essential quality assessment with key recommendations.

---

## **Phase 6: Practical Code Understanding**

**Directive:** Synthesize essential analysis into practical code understanding.

**Essential Understanding Areas:**

1. **Code Structure Summary**: Key components and organization
2. **Dependency Map**: Essential dependencies and relationships
3. **Pattern Map**: Critical patterns and architectural decisions
4. **Functionality Map**: Core functionality and business logic
5. **Quality Map**: Essential quality characteristics and issues
6. **Integration Map**: Key integration points and interactions

**Format:** Concise, practical documentation with key insights and patterns.

---

## **Phase 7: Essential Code Report**

**Directive:** Generate a focused report with practical code insights.

**Report Structure:**

### **Code Structure**
- File type and purpose
- Key classes, methods, and organization
- Framework integration patterns

### **Dependencies**
- Essential external and internal dependencies
- Key relationships and interactions
- Configuration requirements

### **Patterns**
- Critical design and architectural patterns
- Framework-specific patterns and conventions
- Best practices and standards

### **Functionality**
- Core purpose and responsibilities
- Key algorithms and data processing
- Essential business logic

### **Quality**
- Code quality characteristics
- Performance considerations
- Security patterns and issues

### **Practical Insights**
- Key code characteristics
- Critical optimization opportunities
- Essential maintenance considerations

---

## **Usage Examples**

- `/analyze-code-shallow @src/components/UserProfile` - Quick component analysis
- `/analyze-code-shallow @src/services/UserService` - Analyze service patterns
- `/analyze-code-shallow @src/utils/helpers` - Analyze utility functions
- `/analyze-code-shallow @src/guards/AuthGuard` - Analyze guard implementation

## **Shallow Analysis Features**

- **Essential Focus**: Concentrates on critical code patterns and functionality
- **Context Efficient**: Minimizes detailed analysis while maintaining usefulness
- **Practical Insights**: Provides actionable information for development
- **Quick Overview**: Fast analysis suitable for initial code understanding
- **Pattern Recognition**: Identifies key design and architectural patterns
- **Quality Assessment**: Provides essential quality and performance insights

**Remember**: This command provides essential code understanding efficiently, focusing on the most critical patterns and functionality while minimizing context usage.
