# Shallow Analysis Bug Fix Protocol

## Overview

Execute focused bug analysis using streamlined reconnaissance and essential pattern detection. This command provides practical bug insights while minimizing context usage, focusing on the most critical code patterns and dependencies to identify potential bug causes.

## Mission Briefing: Shallow Analysis Bug Fix Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-general`, `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute a focused bug analysis using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows streamlined reconnaissance principles for essential bug understanding. The goal is to identify potential bug causes through systematic analysis of the target file and its ecosystem.

---

## **Phase 0: Target File Analysis (Read-Only)**

**Directive:** Perform focused analysis of the buggy file to understand its structure, purpose, and immediate context.

**Essential Analysis Scope:**

- **File Structure**: Class/module organization, key methods, properties
- **Core Logic**: Main functionality, business rules, algorithms
- **Error Handling**: Try-catch blocks, validation, error states
- **State Management**: Variables, state changes, side effects
- **Input/Output**: Parameters, return values, data flow

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus on understanding the file's purpose and logic**
- **Identify potential failure points and edge cases**

---

## **Phase 1: Consumer Analysis**

**Directive:** Analyze all files and classes that use the buggy file to understand usage patterns and expectations.

**Analysis Areas:**

1. **Direct Consumers**:
   - Find all files that import or use the buggy file
   - Analyze how the buggy file is being called
   - Identify expected behavior vs actual behavior
   - Map usage patterns and data flow

2. **Usage Context**:
   - Understand the business context of usage
   - Identify critical usage paths
   - Map error scenarios and edge cases
   - Analyze input data patterns

**Output:** Complete understanding of how the buggy file is used and what consumers expect.

---

## **Phase 2: Deep Dependency Analysis**

**Directive:** Deeply analyze all dependencies of the buggy file to understand the complete ecosystem.

**Dependency Analysis Areas:**

1. **Direct Dependencies**:
   - All imports and external dependencies
   - Service dependencies and injected services
   - External library dependencies
   - Configuration dependencies

2. **Transitive Dependencies**:
   - Dependencies of dependencies (second-level analysis)
   - Critical dependency chains and their impact
   - Version compatibility issues
   - Potential breaking changes

3. **Dependency Implementation Analysis**:
   - Read and analyze each dependency's implementation
   - Understand what each dependency provides
   - Map dependency interfaces and contracts
   - Analyze dependency behavior and side effects

**Output:** Complete understanding of the dependency ecosystem and potential failure points.

---

## **Phase 3: Business Logic Analysis**

**Directive:** Understand the business logic and algorithms in all analyzed files to identify potential logical errors.

**Business Logic Analysis Areas:**

1. **Algorithm Analysis**:
   - Identify algorithms and their complexity
   - Analyze data structures and their usage
   - Map computational patterns and optimizations
   - Understand sorting, searching, and filtering logic

2. **Data Flow Analysis**:
   - Map data input sources and validation
   - Trace data transformations and processing
   - Identify output destinations and formatting
   - Analyze data persistence and state management

3. **Business Rule Analysis**:
   - Identify business rules and constraints
   - Map validation logic and error conditions
   - Analyze business process flows
   - Understand decision trees and branching logic

4. **Integration Logic Analysis**:
   - Map external system integration patterns
   - Analyze API communication and data exchange
   - Understand event handling and messaging
   - Map workflow and process orchestration

**Output:** Complete understanding of business logic and potential logical errors.

---

## **Phase 4: Bug Hypothesis Formation**

**Directive:** Based on all analysis, form hypotheses about what the bug could be.

**Hypothesis Formation Process:**

1. **Pattern Recognition**:
   - Look for common bug patterns in the analyzed code
   - Identify potential race conditions or timing issues
   - Check for null/undefined handling issues
   - Look for type mismatches or casting problems

2. **Logic Analysis**:
   - Identify potential logical errors in algorithms
   - Check for off-by-one errors or boundary conditions
   - Look for incorrect conditional logic
   - Analyze state management issues

3. **Dependency Issues**:
   - Check for version compatibility problems
   - Look for API changes or breaking changes
   - Identify missing or incorrect dependency usage
   - Check for circular dependency issues

4. **Data Flow Issues**:
   - Look for data corruption or transformation errors
   - Check for incorrect data validation
   - Identify missing error handling
   - Analyze input/output mismatches

**Output:** Ranked list of potential bug causes with supporting evidence.

---

## **Phase 5: Findings Presentation**

**Directive:** Present comprehensive findings and reasoning for bug hypotheses.

**Presentation Structure:**

### **Analysis Summary**
- **Target File**: Brief description of the buggy file's purpose
- **Key Dependencies**: Critical dependencies and their roles
- **Usage Patterns**: How the file is used by consumers
- **Business Logic**: Core business rules and algorithms

### **Potential Bug Causes** (Ranked by Likelihood)

#### **High Probability**
- **Bug Type**: [Type of bug]
- **Location**: [Specific file/line/function]
- **Evidence**: [Supporting evidence from analysis]
- **Reasoning**: [Why this is likely the cause]

#### **Medium Probability**
- **Bug Type**: [Type of bug]
- **Location**: [Specific file/line/function]
- **Evidence**: [Supporting evidence from analysis]
- **Reasoning**: [Why this could be the cause]

#### **Low Probability**
- **Bug Type**: [Type of bug]
- **Location**: [Specific file/line/function]
- **Evidence**: [Supporting evidence from analysis]
- **Reasoning**: [Why this might be the cause]

### **Recommended Next Steps**
1. **Immediate Actions**: [What to check first]
2. **Debugging Strategy**: [How to confirm the hypothesis]
3. **Testing Approach**: [How to reproduce and verify the fix]
4. **Additional Analysis**: [What else might need investigation]

---

## **Phase 6: User Feedback & Direction**

**Directive:** Present findings and wait for user approval and/or feedback on further direction.

**User Interaction Protocol:**

1. **Present Findings**: Share the complete analysis and bug hypotheses
2. **Request Feedback**: Ask for user input on the analysis
3. **Clarify Questions**: Answer any questions about the findings
4. **Refine Analysis**: Adjust analysis based on user feedback
5. **Wait for Direction**: Pause for user approval or further instructions

**Expected User Responses:**
- **Approval**: User agrees with analysis and wants to proceed with fix
- **Refinement**: User wants to focus on specific areas or hypotheses
- **Additional Analysis**: User wants deeper investigation of specific aspects
- **Different Approach**: User wants to try a different analysis approach

---

## **Usage Examples**

- `/shallow-analysis-bug-fix Analyze the UserService.ts file for authentication issues`
- `/shallow-analysis-bug-fix Investigate the DataProcessor class for data corruption bugs`
- `/shallow-analysis-bug-fix Analyze the PaymentHandler for transaction processing errors`
- `/shallow-analysis-bug-fix Investigate the FileUploader component for upload failures`

## **Analysis Features**

- **Focused Analysis**: Quick insights with minimal context usage
- **Dependency Mapping**: Complete understanding of the code ecosystem
- **Business Logic Analysis**: Deep understanding of algorithms and business rules
- **Hypothesis Formation**: Systematic approach to identifying potential bug causes
- **Evidence-Based Reasoning**: All conclusions supported by analysis evidence
- **User Collaboration**: Interactive process with user feedback and direction

**Remember**: This command provides focused bug analysis with minimal context usage while maintaining comprehensive understanding of the code ecosystem. The goal is to identify potential bug causes through systematic analysis and present findings for user review and direction.
