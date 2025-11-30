# Create Refactor Plan Command

## Overview
Create a comprehensive, detailed refactoring plan document that follows the planning document structure but focuses specifically on refactoring work. The refactoring design must be **VERY detailed** with specific code changes, before/after examples, and step-by-step refactoring strategies. This enables systematic refactoring execution with complete context preservation across AI sessions.

## Mission Briefing: Refactoring Plan Protocol

You will now transition to **Refactoring Plan Architect** mode. Your mission is to create a comprehensive, detailed refactoring plan document that preserves all critical refactoring context and provides clear, actionable refactoring steps for future execution.

**Your goal is to create a detailed refactoring plan document that enables systematic refactoring execution with complete understanding of current state, target state, and all refactoring steps in between.**

---

## **CRITICAL DIRECTIVE: Refactoring Plan Creation**

**This command creates detailed refactoring plan documents (`.refactor-plan.md` files):**

1. **ANALYZE target code** comprehensively (read-only reconnaissance)
2. **ANALYZE ENTIRE CODEBASE** for project-wide context, existing components, and reuse opportunities (CRITICAL)
3. **IDENTIFY extractable components** that should become shared/dedicated components
4. **SEARCH for existing components** that could be reused, merged, or extended
5. **REQUEST USER DECISIONS** for component reuse/merge/create options
6. **IDENTIFY all refactoring opportunities** with detailed analysis
7. **CREATE detailed refactoring plan** with before/after examples
8. **DOCUMENT all refactoring steps** with specific code changes
9. **PROVIDE clear execution path** for systematic refactoring

**CRITICAL REQUIREMENTS**:
- **Project-Wide Context**: The refactoring plan MUST consider the entire codebase, not just the target component
- **Component Reuse**: MUST identify opportunities to extract code into shared components
- **Existing Component Discovery**: MUST search for and compare with existing components
- **User Decision Points**: MUST present reuse/merge/create options and wait for user input
- **Component Extraction**: MUST convert extractable sections into shared/dedicated components

**Remember**: The refactoring plan is the PRIMARY source of truth for all refactoring work. It must be **VERY detailed** with specific code examples and step-by-step instructions. The plan must think about the project as a whole, not just the target component.

---

## **Refactoring Plan Capture Protocol**

**Directive:** Analyze the target code and create a comprehensive refactoring plan document in the following structured format:

### **Pre-Plan Setup**

1. **Identify Target Code:** Determine what code/files need refactoring
2. **Determine File Path:** 
   - New: `planning/[ticket-id]/[feature-name].refactor-plan.md`
   - Or: `planning/refactoring/[target-name].refactor-plan.md`
3. **Project-Wide Context Analysis:** Analyze entire codebase for patterns, existing components, and reuse opportunities
4. **Analyze Current State:** Comprehensive read-only analysis of current code
5. **Gather Context:** Collect all dependencies, usage patterns, test coverage
6. **Component Discovery:** Search for existing components that could be reused or merged

### **Refactoring Plan Document Structure**

Create refactoring plan document with the following sections:

#### 1. Refactoring Overview (REQUIRED)

**High-level summary, goals, scope, and success criteria**

#### 2. Current State Analysis (REQUIRED)

**Detailed analysis of current code state, code smells, and issues**

#### 2a. Project-Wide Context Analysis (REQUIRED)

**Analysis of entire codebase for component reuse opportunities, existing patterns, and shared code**

#### 3. Component Extraction & Reuse Analysis (REQUIRED)

**Identification of extractable components, existing component discovery, and reuse decision points**

#### 4. Target State Design (REQUIRED)

**Detailed design of refactored code with architecture decisions**

#### 5. Refactoring Strategy (REQUIRED)

**Detailed step-by-step refactoring plan with specific code changes**

#### 6. Code Examples (REQUIRED)

**Before/after code examples for each major refactoring**

#### 7. Testing Strategy (REQUIRED)

**How to test refactored code and ensure no regressions**

#### 8. Risk Assessment (REQUIRED)

**Risks, mitigation strategies, and rollback plans**

---

## **Output Format**

Create a refactoring plan document with the following structure:

```markdown
# [Target Name] - Refactoring Plan

## Refactoring Overview

**Status**: [Planning Complete | In Progress | Blocked | Complete]

**Last Updated**: [Date and Time]

**Target Scope**: 
- Files: `path/to/file1.ts`, `path/to/file2.tsx`
- Components: [List of components/modules]
- Estimated Complexity: [Low | Medium | High]
- Estimated Time: [X hours/days]

**Refactoring Goals**:
1. **Readability**: [Specific readability improvements]
2. **Maintainability**: [Specific maintainability improvements]
3. **Testability**: [Specific testability improvements]
4. **SOLID Compliance**: [Specific SOLID improvements]
5. **DRY Compliance**: [Specific DRY improvements]
6. **YAGNI Compliance**: [Remove unnecessary complexity]

**Success Criteria**:
- [ ] Code is more readable (self-documenting)
- [ ] Code is easier to maintain (clear boundaries)
- [ ] Code is fully testable (all paths testable)
- [ ] SOLID principles followed (single responsibility, etc.)
- [ ] No code duplication (DRY compliance)
- [ ] No over-engineering (YAGNI compliance)
- [ ] All tests pass (no regressions)
- [ ] Performance maintained or improved

---

## Current State Analysis

### Code Smells Identified

#### Smell 1: [Name] - [Severity: High/Medium/Low]
**Location**: `path/to/file.ts:123-145`

**Description**: 
[Detailed description of the code smell]

**Current Code**:
\`\`\`typescript
// Current implementation
// Line 123-145
function complexFunction(param1: any, param2: any, param3: any) {
  // 50+ lines of complex logic
  if (condition1) {
    // Nested logic
    if (condition2) {
      // More nesting
    }
  }
  // More complex logic...
}
\`\`\`

**Issues**:
- [ ] Violates Single Responsibility (does multiple things)
- [ ] Poor readability (unclear intent)
- [ ] Hard to test (tight coupling)
- [ ] High complexity (cyclomatic complexity: X)
- [ ] Too many parameters (3 parameters, should be ≤2)
- [ ] Long function (50+ lines, should be ≤20)

**Impact**:
- **Readability**: [How it affects readability]
- **Maintainability**: [How it affects maintainability]
- **Testability**: [How it affects testability]
- **Dependencies**: [What depends on this code]

#### Smell 2: [Name] - [Severity: High/Medium/Low]
[Repeat structure for each code smell]

### Architecture Issues

#### Issue 1: [Name]
**Description**: [Detailed description]

**Current Structure**:
\`\`\`typescript
// Current architecture
// Show dependency graph or structure
\`\`\`

**Problems**:
- [ ] Tight coupling between [Component A] and [Component B]
- [ ] Circular dependency between [Module A] and [Module B]
- [ ] No clear layer separation
- [ ] Dependencies point in wrong direction

**Impact**: [How this affects maintainability and testability]

### SOLID Violations

#### Violation 1: Single Responsibility Principle
**Location**: `path/to/file.ts:Class Name`

**Description**: [Class/component has multiple responsibilities]

**Responsibilities Identified**:
1. [Responsibility 1]
2. [Responsibility 2]
3. [Responsibility 3]

**Current Code**:
\`\`\`typescript
// Show current code with multiple responsibilities
\`\`\`

#### Violation 2: Dependency Inversion Principle
[Repeat for each SOLID violation]

### DRY Violations

#### Duplication 1: [Name]
**Locations**: 
- `path/to/file1.ts:45-60`
- `path/to/file2.ts:120-135`
- `path/to/file3.ts:200-215`

**Duplicated Code**:
\`\`\`typescript
// Show duplicated code pattern
function validateEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}
// This appears in 3 different files
\`\`\`

**Abstraction Opportunity**: [How to extract this]

### Testability Issues

#### Issue 1: [Name]
**Location**: `path/to/file.ts`

**Description**: [Why this code is hard to test]

**Problems**:
- [ ] Hard-coded dependencies
- [ ] Side effects in business logic
- [ ] Global state dependencies
- [ ] No dependency injection

---

## Project-Wide Context Analysis

**CRITICAL**: This analysis MUST examine the entire codebase, not just the target component. The refactoring plan must consider project-wide patterns, existing components, and reuse opportunities.

### Codebase Component Inventory

**Search Strategy**:
1. **Component Directory Analysis**: Search `src/components/`, `components/`, `app/components/`, etc.
2. **Pattern Matching**: Look for similar component patterns, naming conventions, and structures
3. **Shared Utilities**: Identify shared utilities, hooks, and helper functions
4. **Design System**: Identify existing design system components and patterns
5. **Theming System Analysis**: Identify theming patterns, theme hooks, ThemedText/ThemedTextInput usage, and color system

**Existing Components Found**:
- **Component 1**: `path/to/ExistingComponent.tsx`
  - **Purpose**: [What this component does]
  - **Similarity to Target**: [How similar to code being refactored]
  - **Reuse Potential**: [High/Medium/Low - why]
  - **API/Props**: [What props/API does it expose]
  - **Theming Compliance**: [✅ Compliant | ⚠️ Partial | ❌ Non-compliant]
    - **ThemedText Usage**: [Uses ThemedText wrapper? Yes/No]
    - **Theme Hook**: [Uses useTheme or theme context?]
    - **Color System**: [Uses theme.colors.* or hardcoded colors?]
    - **Typography**: [Follows M3 typography scale?]
  
- **Component 2**: `path/to/AnotherComponent.tsx`
  - **Purpose**: [What this component does]
  - **Similarity to Target**: [How similar to code being refactored]
  - **Reuse Potential**: [High/Medium/Low - why]
  - **API/Props**: [What props/API does it expose]
  - **Theming Compliance**: [✅ Compliant | ⚠️ Partial | ❌ Non-compliant]
    - **ThemedText Usage**: [Uses ThemedText wrapper? Yes/No]
    - **Theme Hook**: [Uses useTheme or theme context?]
    - **Color System**: [Uses theme.colors.* or hardcoded colors?]
    - **Typography**: [Follows M3 typography scale?]

### Shared Patterns & Utilities

**Common Patterns Identified**:
- **Pattern 1**: [Description of pattern found across codebase]
  - **Locations**: `path/to/file1.ts`, `path/to/file2.tsx`
  - **Relevance**: [How this relates to refactoring target]
  
- **Pattern 2**: [Description of pattern]
  - **Locations**: [List of locations]
  - **Relevance**: [How this relates to refactoring target]

**Shared Utilities Found**:
- **Utility 1**: `path/to/shared-utility.ts`
  - **Functionality**: [What it does]
  - **Reuse Opportunity**: [Can target code use this?]
  
- **Utility 2**: `path/to/another-utility.ts`
  - **Functionality**: [What it does]
  - **Reuse Opportunity**: [Can target code use this?]

### Theming System Analysis

**CRITICAL**: All refactored components MUST comply with the project's theming system (Material Design 3).

**Theming Patterns Identified**:
- **Theme Hook**: `useTheme()` or theme context pattern
  - **Location**: `path/to/theme/hook.ts` or `path/to/ThemeContext.tsx`
  - **Usage Pattern**: [How components access theme]
  
- **ThemedText Component**: Wrapper for text rendering
  - **Location**: `path/to/components/common/ThemedText.tsx`
  - **Variants**: [List available variants: title, body, bodySmall, etc.]
  - **Usage Pattern**: [How components use ThemedText]
  
- **ThemedTextInput Component**: Wrapper for text inputs
  - **Location**: `path/to/components/common/ThemedTextInput.tsx`
  - **Variants**: [List available variants]
  - **Usage Pattern**: [How components use ThemedTextInput]
  
- **Color System**: Theme color roles
  - **Pattern**: `theme.colors.primary`, `theme.colors.surface`, etc.
  - **Hardcoded Colors Found**: [List any hardcoded colors that should use theme]
  - **Compliance**: [Overall theming compliance level]

**Target Code Theming Analysis**:
- **Current Theming Compliance**: [✅ Compliant | ⚠️ Partial | ❌ Non-compliant]
- **Issues Found**:
  - [ ] Uses direct `Text` instead of `ThemedText`
  - [ ] Uses direct `TextInput` instead of `ThemedTextInput`
  - [ ] Hardcoded colors instead of `theme.colors.*`
  - [ ] Hardcoded font sizes/weights instead of ThemedText variants
  - [ ] Missing theme hook usage
  - [ ] Non-M3 typography patterns

**Theming Migration Requirements**:
- [ ] Replace all `Text` with `ThemedText` with appropriate variants
- [ ] Replace all `TextInput` with `ThemedTextInput` with appropriate variants
- [ ] Replace hardcoded colors with `theme.colors.*`
- [ ] Remove `fontFamily` from StyleSheet (use ThemedText variants)
- [ ] Use M3 typography scale (22/16/14/12px via ThemedText variants)
- [ ] Use M3 spacing scale (16/24/32px)
- [ ] Use M3 color roles (primary, surface, onPrimary, etc.)

### Component Extraction Opportunities

**Extractable Sections Identified**:

#### Extractable Component 1: [Component Name]
**Location in Target Code**: `path/to/target-file.tsx:45-120`

**Current Code**:
\`\`\`typescript
// Show code that could be extracted
// This section appears to be a reusable component
function renderUserCard(user: User) {
  return (
    <View style={styles.card}>
      <Text>{user.name}</Text>
      <Text>{user.email}</Text>
    </View>
  );
}
\`\`\`

**Extraction Rationale**:
- [ ] Appears in multiple places (DRY violation)
- [ ] Has clear, single responsibility
- [ ] Could be reused elsewhere in codebase
- [ ] Would improve testability
- [ ] Would improve maintainability

**Theming Compliance**:
- **Current State**: [✅ Compliant | ⚠️ Partial | ❌ Non-compliant]
- **Theming Issues**:
  - [ ] Uses direct `Text` instead of `ThemedText`
  - [ ] Uses direct `TextInput` instead of `ThemedTextInput`
  - [ ] Hardcoded colors
  - [ ] Non-M3 typography patterns
- **Theming Migration Needed**: [Yes/No - if yes, describe what needs to be fixed]

**Similarity to Existing Components**: [Compare to existing components found]
- **Theming Comparison**: [Do existing similar components follow theming? If reusing, will theming be consistent?]

**Decision Required**: ⚠️ **USER INPUT NEEDED**
- [ ] **Option A**: Reuse existing component `[ComponentName]` (if similar exists)
  - **Rationale**: [Why reuse makes sense]
  - **Changes Needed**: [What modifications needed to existing component]
  - **Theming Considerations**: 
    - [ ] Existing component theming compliance: [✅/⚠️/❌]
    - [ ] Theming migration needed: [Yes/No - describe]
    - [ ] Will ensure theming consistency: [Yes/No]
  - **Impact**: [What this affects]
  
- [ ] **Option B**: Merge functionality into existing component `[ComponentName]`
  - **Rationale**: [Why merge makes sense]
  - **Changes Needed**: [What modifications needed]
  - **Theming Considerations**:
    - [ ] Existing component theming compliance: [✅/⚠️/❌]
    - [ ] Theming migration needed for merged code: [Yes/No - describe]
    - [ ] Will ensure unified theming: [Yes/No]
  - **Impact**: [What this affects]
  
- [ ] **Option C**: Create new shared component `[NewComponentName]`
  - **Rationale**: [Why new component makes sense]
  - **Location**: `src/components/[NewComponentName]/`
  - **API Design**: [What props/interface it should have]
  - **Theming Requirements**:
    - [ ] MUST use `ThemedText` for all text rendering
    - [ ] MUST use `ThemedTextInput` for all text inputs
    - [ ] MUST use `theme.colors.*` for all colors
    - [ ] MUST follow M3 typography scale via ThemedText variants
    - [ ] MUST follow M3 spacing scale (16/24/32px)
    - [ ] MUST use theme hook (`useTheme()`)
  - **Impact**: [What this affects]

**Recommendation**: [LLM's recommendation with rationale]

#### Extractable Component 2: [Component Name]
[Repeat structure for each extractable component]

### Project-Wide Impact Analysis

**Files That May Be Affected**:
- `path/to/file1.tsx` - [How it might be affected]
- `path/to/file2.tsx` - [How it might be affected]

**Dependencies to Consider**:
- [Dependency 1]: [How it relates to refactoring]
- [Dependency 2]: [How it relates to refactoring]

**Shared Code Locations**:
- `path/to/shared/` - [What shared code exists here]
- `path/to/common/` - [What common code exists here]

---

## Component Extraction & Reuse Analysis

### Component Extraction Strategy

**MANDATORY PROCESS**: For each extractable component identified, the LLM MUST:

1. **Search for Existing Components**: Use codebase search to find similar components
2. **Compare Functionality**: Analyze if existing component can be reused
3. **Assess Merge Feasibility**: Determine if functionality can be merged
4. **Request User Decision**: Present options and ask user to decide
5. **Document Decision**: Record user's decision in the plan

### Extraction Candidates

#### Candidate 1: [Component Name]
**Source Location**: `path/to/target-file.tsx:45-120`

**Functionality**:
- [Description of what this component does]
- [Key features and responsibilities]

**Existing Component Search Results**:
- **Found**: `path/to/SimilarComponent.tsx`
  - **Similarity Score**: [High/Medium/Low]
  - **Functional Overlap**: [What overlaps]
  - **Differences**: [What's different]
  - **API Comparison**: [Compare APIs/props]

**Decision Matrix**:

| Option | Pros | Cons | Recommendation |
|--------|------|------|----------------|
| **Reuse Existing** | [Benefits] | [Drawbacks] | [Yes/No/Maybe] |
| **Merge Functionality** | [Benefits] | [Drawbacks] | [Yes/No/Maybe] |
| **Create New** | [Benefits] | [Drawbacks] | [Yes/No/Maybe] |

**⚠️ USER DECISION REQUIRED**:
> **Question for User**: Should we:
> 1. **Reuse** `[ExistingComponent]` with modifications? (Specify what modifications)
> 2. **Merge** functionality into `[ExistingComponent]`? (Specify merge approach)
> 3. **Create** new shared component `[NewComponentName]`? (Specify API design)

**Decision**: [To be filled after user input]
**Rationale**: [Why this decision was made]

#### Candidate 2: [Component Name]
[Repeat structure for each candidate]

### Shared Component Integration Plan

**Components to Create**:
- `src/components/[NewComponent]/[NewComponent].tsx`
  - **Purpose**: [What it does]
  - **API**: [Props/interface design]
  - **Dependencies**: [What it depends on]
  - **Usage Locations**: [Where it will be used]

**Components to Modify**:
- `src/components/[ExistingComponent]/[ExistingComponent].tsx`
  - **Modifications**: [What changes needed]
  - **Backward Compatibility**: [Will this break existing usage?]
  - **Migration Path**: [How to migrate existing usages]

**Components to Deprecate**:
- `src/components/[OldComponent]/[OldComponent].tsx`
  - **Reason**: [Why deprecating]
  - **Replacement**: [What replaces it]
  - **Migration Timeline**: [When to migrate]

### Component Reuse Validation

**Reuse Checklist**:
- [ ] Component has single, clear responsibility
- [ ] Component API is well-defined and stable
- [ ] Component is properly tested
- [ ] Component follows project conventions
- [ ] Component is documented
- [ ] Reuse won't create circular dependencies
- [ ] Reuse won't break existing functionality
- [ ] **Component follows theming system** (M3 compliance)
  - [ ] Uses `ThemedText` for all text rendering
  - [ ] Uses `ThemedTextInput` for all text inputs
  - [ ] Uses `theme.colors.*` for all colors (no hardcoded colors)
  - [ ] Follows M3 typography scale via ThemedText variants
  - [ ] Follows M3 spacing scale (16/24/32px)
  - [ ] Uses theme hook (`useTheme()`) for theme access

---

## Target State Design

### Architecture Design

**Target Architecture**:
\`\`\`typescript
// Show target architecture structure
// Layer separation
// Dependency direction
// Component organization
\`\`\`

**Key Design Decisions**:
1. **Decision 1**: [Description]
   - **Rationale**: [Why this decision]
   - **Impact**: [What this affects]
   - **Trade-offs**: [Any trade-offs]

2. **Decision 2**: [Description]
   - **Rationale**: [Why this decision]
   - **Impact**: [What this affects]

3. **Theming Compliance**: All refactored components MUST follow M3 theming system
   - **Rationale**: Ensures consistency with project design system and maintainability
   - **Impact**: All components will use ThemedText, theme colors, and M3 patterns
   - **Trade-offs**: May require additional migration work, but ensures long-term consistency

### Component/Module Structure

**Target Structure**:
```
src/
  ├── components/
  │   ├── [Component1]/
  │   │   ├── Component1.tsx
  │   │   ├── Component1.test.tsx
  │   │   └── Component1.types.ts
  │   └── [Component2]/
  ├── services/
  │   ├── [Service1].ts
  │   └── [Service2].ts
  ├── utils/
  │   └── [Utility].ts
  └── types/
      └── [Types].ts
```

**Layer Separation**:
- **Presentation Layer**: [Components, UI logic]
- **Business Logic Layer**: [Services, domain logic]
- **Data Access Layer**: [API calls, data fetching]

**Dependency Direction**: [Inner layers don't depend on outer layers]

### SOLID Compliance Design

#### Single Responsibility
**Target**: Each class/function has one clear responsibility

**Example**:
\`\`\`typescript
// Target: Focused service
class UserDataService {
  // Only handles user data operations
  async getUser(id: string): Promise<User> { }
  async saveUser(user: User): Promise<void> { }
}

class UserValidationService {
  // Only handles user validation
  validateUser(user: User): ValidationResult { }
}
\`\`\`

#### Dependency Inversion
**Target**: Depend on abstractions, not concretions

**Example**:
\`\`\`typescript
// Target: Interface-based dependency
interface IUserRepository {
  getUser(id: string): Promise<User>;
}

class UserService {
  constructor(private repository: IUserRepository) {}
  // Depends on interface, not concrete implementation
}
\`\`\`

### Readability Improvements

**Target Naming Conventions**:
- Functions: `verbNoun` (e.g., `getUser`, `validateEmail`)
- Classes: `Noun` (e.g., `UserService`, `EmailValidator`)
- Variables: `noun` or `adjectiveNoun` (e.g., `user`, `isValid`)
- Constants: `UPPER_SNAKE_CASE` (e.g., `MAX_RETRY_COUNT`)

**Target Function Length**: ≤20 lines (prefer 5-15 lines)

**Target Complexity**: Cyclomatic complexity ≤10 per function

---

## Refactoring Strategy

### Phase 1: Preparation (Read-Only Analysis)
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete

**Tasks**:
- [ ] Task 1.1: Analyze current code structure
  - Files: `path/to/file.ts`
  - Action: Read and understand current implementation
  - Output: Code smell inventory
- [ ] Task 1.2: **Project-Wide Component Discovery** (CRITICAL)
  - **Action**: Search entire codebase for existing components
    - Search directories: `src/components/`, `components/`, `app/components/`, etc.
    - Use semantic search to find similar component patterns
    - Identify shared utilities, hooks, and helper functions
  - **Output**: Component inventory with similarity analysis
  - **Tools**: Use `codebase_search` and `grep` to find components
  - **Estimated Time**: 20-30 minutes
- [ ] Task 1.3: Identify extractable component sections
  - Files: Target files being refactored
  - Action: Identify code sections that could become reusable components
  - Output: List of extractable components with locations
  - **Criteria**: 
    - Code that appears in multiple places (DRY)
    - Code with clear, single responsibility
    - Code that could be reused elsewhere
- [ ] Task 1.4: Compare extractable components with existing components
  - Action: For each extractable component, compare with existing components found
  - Output: Similarity analysis and reuse feasibility assessment
  - **Decision Points**: Identify where user input is needed
- [ ] Task 1.5: Identify all dependencies
  - Files: All target files
  - Action: Map component/service dependencies
  - Output: Dependency graph
- [ ] Task 1.6: Review test coverage
  - Files: `**/*.test.ts`, `**/*.spec.ts`
  - Action: Identify existing tests and coverage gaps
  - Output: Test coverage report
- [ ] Task 1.7: Document current behavior
  - Action: Create characterization tests if needed
  - Output: Test suite that documents current behavior

### Phase 1.5: Component Extraction & Reuse Decisions (USER INPUT REQUIRED)
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete | ⚠️ Awaiting User Input

**CRITICAL**: This phase requires user decisions before proceeding. The LLM MUST present options and wait for user input.

**Tasks**:
- [ ] Task 1.5.1: Present component extraction options to user
  - **For each extractable component identified**:
    - Show existing similar components found
    - Present three options: Reuse, Merge, or Create New
    - Provide decision matrix with pros/cons
    - **⚠️ WAIT FOR USER DECISION** before proceeding
  - **Output**: User decisions documented in plan
  - **Estimated Time**: 10-15 minutes (depends on user response time)

- [ ] Task 1.5.2: Document user decisions
  - **Action**: Record user's decision for each component
  - **Output**: Updated plan with decisions and rationale
  - **Next Steps**: Proceed to Phase 2 with documented decisions

**User Decision Template** (to be filled during planning):
```
Component: [ComponentName]
Location: path/to/target-file.tsx:45-120

User Decision: [Reuse Existing | Merge | Create New]
Decision Rationale: [Why this decision was made]
Action Items: [What needs to be done based on decision]
```

### Phase 2: Extract Abstractions (DRY & SOLID)
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete

**Note**: This phase proceeds AFTER user decisions in Phase 1.5 are documented.

**Tasks**:
- [ ] Task 2.1: Extract duplicated validation logic
  - **File**: `src/utils/validation.ts` (new)
  - **Current Code**:
    \`\`\`typescript
    // Show duplicated code from file1.ts:45-60
    function validateEmail(email: string): boolean {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(email);
    }
    \`\`\`
  - **Target Code**:
    \`\`\`typescript
    // New file: src/utils/validation.ts
    export function validateEmail(email: string): boolean {
      const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return EMAIL_REGEX.test(email);
    }
    \`\`\`
  - **Changes**:
    1. Create new file `src/utils/validation.ts`
    2. Extract function with improved naming (EMAIL_REGEX constant)
    3. Export function
    4. Update imports in file1.ts, file2.ts, file3.ts
    5. Remove duplicated code from all three files
  - **Verification**: Run tests, verify behavior unchanged
  - **Estimated Time**: 15-20 minutes

- [ ] Task 2.2: Extract interface for dependency inversion
  - **File**: `src/types/IUserRepository.ts` (new)
  - **Current Code**:
    \`\`\`typescript
    // Show current concrete dependency
    class UserService {
      private repository = new UserRepository();
    }
    \`\`\`
  - **Target Code**:
    \`\`\`typescript
    // New file: src/types/IUserRepository.ts
    export interface IUserRepository {
      getUser(id: string): Promise<User>;
      saveUser(user: User): Promise<void>;
    }
    
    // Updated: src/services/UserService.ts
    class UserService {
      constructor(private repository: IUserRepository) {}
    }
    \`\`\`
  - **Changes**:
    1. Create interface `IUserRepository`
    2. Update `UserRepository` to implement interface
    3. Update `UserService` to accept interface in constructor
    4. Update all instantiations to inject dependency
  - **Verification**: Update tests with mocks, verify behavior
  - **Estimated Time**: 20-30 minutes

- [ ] Task 2.3: **Extract/Reuse Components** (Based on User Decisions from Phase 1.5)
  - **CRITICAL**: This task executes based on user decisions documented in Phase 1.5
  
  **For each component extraction decision**:
  
  **If Decision = "Reuse Existing"**:
  - **File**: `path/to/existing-component.tsx` (modify)
  - **Action**: 
    1. Review existing component API
    2. Modify existing component to support new use case (if needed)
    3. Update target code to use existing component
    4. Ensure backward compatibility
  - **Changes**:
    - [List specific changes to existing component]
    - [List changes to target code to use existing component]
  - **Verification**: 
    - Test existing component still works for current usages
    - Test new usage in target code
    - Run full test suite
  - **Estimated Time**: 30-45 minutes per component
  
  **If Decision = "Merge Functionality"**:
  - **File**: `path/to/existing-component.tsx` (modify)
  - **Action**:
    1. Analyze functionality to merge
    2. Design merged component API
    3. Implement merged functionality
    4. Update all usages (existing + new)
    5. Deprecate old component if applicable
  - **Changes**:
    - [List specific merge changes]
    - [List migration steps for existing usages]
  - **Verification**:
    - Test merged component with all use cases
    - Verify no regressions in existing usages
    - Run full test suite
  - **Estimated Time**: 45-60 minutes per component
  
  **If Decision = "Create New"**:
  - **File**: `src/components/[NewComponent]/[NewComponent].tsx` (new)
  - **Action**:
    1. Create new component directory structure
    2. Implement component based on extracted code
    3. Design component API (props, types)
    4. Write component tests
    5. Update target code to use new component
    6. Document component usage
  - **Current Code** (from target):
    \`\`\`typescript
    // Show code being extracted
    // This will become the new component
    \`\`\`
  - **Target Code** (new component):
    \`\`\`typescript
    // New file: src/components/[NewComponent]/[NewComponent].tsx
    export interface [NewComponent]Props {
      // Define props interface
    }
    
    export const [NewComponent] = (props: [NewComponent]Props) => {
      // Component implementation
    };
    \`\`\`
  - **Changes**:
    1. Create `src/components/[NewComponent]/` directory
    2. Create `[NewComponent].tsx` with extracted code
    3. Create `[NewComponent].types.ts` with TypeScript types
    4. Create `[NewComponent].test.tsx` with tests
    5. Update target code to import and use new component
    6. Remove extracted code from target file
  - **Verification**:
    - Test new component in isolation
    - Test new component in target code context
    - Verify no regressions
    - Run full test suite
  - **Estimated Time**: 45-60 minutes per component

- [ ] Task 2.4: **Apply Theming System** (M3 Compliance)
  - **CRITICAL**: All refactored/extracted components MUST comply with theming system
  - **Files**: All components created/modified in Phase 2
  - **Action**: Migrate all components to use theming system
  
  **Theming Migration Steps**:
  1. **Replace Text with ThemedText**:
     - **Current Code**:
       \`\`\`typescript
       <Text style={styles.text}>Hello</Text>
       \`\`\`
     - **Target Code**:
       \`\`\`typescript
       import { ThemedText } from '../../components/common';
       <ThemedText variant="body">Hello</ThemedText>
       \`\`\`
     - **Changes**: Replace all `Text` with `ThemedText` using appropriate variants
  
  2. **Replace TextInput with ThemedTextInput**:
     - **Current Code**:
       \`\`\`typescript
       <TextInput style={styles.input} />
       \`\`\`
     - **Target Code**:
       \`\`\`typescript
       import { ThemedTextInput } from '../../components/common';
       <ThemedTextInput variant="body" />
       \`\`\`
     - **Changes**: Replace all `TextInput` with `ThemedTextInput` using appropriate variants
  
  3. **Replace Hardcoded Colors with Theme Colors**:
     - **Current Code**:
       \`\`\`typescript
       const styles = StyleSheet.create({
         container: {
           backgroundColor: '#FFFFFF',
           color: '#000000',
         },
       });
       \`\`\`
     - **Target Code**:
       \`\`\`typescript
       const { theme } = useTheme();
       const styles = StyleSheet.create({
         container: {
           backgroundColor: theme.colors.surface,
           color: theme.colors.textPrimary,
         },
       });
       \`\`\`
     - **Changes**: Replace all hardcoded colors with `theme.colors.*`
  
  4. **Remove fontFamily from StyleSheet**:
     - **Current Code**:
       \`\`\`typescript
       const styles = StyleSheet.create({
         text: {
           fontFamily: 'Inter-Regular',
           fontSize: 16,
           fontWeight: '400',
         },
       });
       \`\`\`
     - **Target Code**:
       \`\`\`typescript
       // Remove fontFamily - use ThemedText variant instead
       <ThemedText variant="body">Text</ThemedText>
       \`\`\`
     - **Changes**: Remove `fontFamily` from StyleSheet, use ThemedText variants
  
  5. **Apply M3 Typography Scale**:
     - Use ThemedText variants: `title` (22px), `body` (16px), `bodySmall` (14px), `bodyTiny` (12px)
     - Use appropriate variant for each text element
  
  6. **Apply M3 Spacing Scale**:
     - Use spacing: 16px, 24px, 32px (avoid 8px, 12px for major spacing)
  
  **Verification**:
  - [ ] All `Text` replaced with `ThemedText`
  - [ ] All `TextInput` replaced with `ThemedTextInput`
  - [ ] No hardcoded colors remain
  - [ ] No `fontFamily` in StyleSheet
  - [ ] M3 typography scale followed
  - [ ] M3 spacing scale followed
  - [ ] Theme hook used where needed
  - [ ] Visual regression testing passes
  - [ ] All tests pass
  
  **Estimated Time**: 30-45 minutes per component (depends on theming compliance level)

### Phase 3: Decompose Large Functions (Readability & Single Responsibility)
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete

**Tasks**:
- [ ] Task 3.1: Extract complex conditional logic
  - **File**: `path/to/file.ts:123-145`
  - **Current Code**:
    \`\`\`typescript
    // Show current long function
    function processUserData(userData: any): void {
      if (userData.type === 'admin') {
        this.setAdminPermissions(userData);
        this.configureAdminUI(userData);
        this.setupAdminNotifications(userData);
      } else if (userData.type === 'user') {
        this.setUserPermissions(userData);
        this.configureUserUI(userData);
        this.setupUserNotifications(userData);
      }
      // 30+ more lines...
    }
    \`\`\`
  - **Target Code**:
    \`\`\`typescript
    // Extract method for admin processing
    private processAdminUser(userData: UserData): void {
      this.setAdminPermissions(userData);
      this.configureAdminUI(userData);
      this.setupAdminNotifications(userData);
    }
    
    // Extract method for regular user processing
    private processRegularUser(userData: UserData): void {
      this.setUserPermissions(userData);
      this.configureUserUI(userData);
      this.setupUserNotifications(userData);
    }
    
    // Simplified main function
    function processUserData(userData: UserData): void {
      if (userData.type === 'admin') {
        this.processAdminUser(userData);
      } else if (userData.type === 'user') {
        this.processRegularUser(userData);
      }
    }
    \`\`\`
  - **Changes**:
    1. Extract `processAdminUser` method
    2. Extract `processRegularUser` method
    3. Simplify main `processUserData` function
    4. Add proper TypeScript types (replace `any` with `UserData`)
    5. Update tests to cover extracted methods
  - **Benefits**:
    - Improved readability (clear method names)
    - Single responsibility (each method does one thing)
    - Easier to test (can test each method independently)
    - Easier to maintain (changes isolated to specific methods)
  - **Verification**: Run tests, verify behavior unchanged
  - **Estimated Time**: 25-30 minutes

### Phase 4: Improve Naming and Readability
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete

**Tasks**:
- [ ] Task 4.1: Rename unclear variables and functions
  - **File**: `path/to/file.ts`
  - **Current Code**:
    \`\`\`typescript
    function proc(data: any): any {
      let temp = data.x + data.y;
      if (temp > 10) {
        return true;
      }
      return false;
    }
    \`\`\`
  - **Target Code**:
    \`\`\`typescript
    function isTotalExceedingThreshold(
      value1: number,
      value2: number,
      threshold: number = 10
    ): boolean {
      const total = value1 + value2;
      return total > threshold;
    }
    \`\`\`
  - **Changes**:
    1. Rename `proc` → `isTotalExceedingThreshold` (clear intent)
    2. Rename `data` → explicit parameters `value1`, `value2`
    3. Rename `temp` → `total` (clear meaning)
    4. Replace magic number `10` with named parameter `threshold`
    5. Replace `any` with proper types
    6. Use early return pattern for clarity
    7. Update all call sites
  - **Verification**: Run tests, verify behavior unchanged
  - **Estimated Time**: 15-20 minutes

### Phase 5: Component/Module Decomposition
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete

**Tasks**:
- [ ] Task 5.1: Split large component into smaller components
  - **File**: `src/components/LargeComponent.tsx`
  - **Current Code**: [Show large component with multiple responsibilities]
  - **Target Structure**:
    ```
    src/components/
      ├── LargeComponent/
      │   ├── LargeComponent.tsx (orchestrator)
      │   ├── LargeComponent.test.tsx
      │   ├── UserInfo/
      │   │   ├── UserInfo.tsx
      │   │   └── UserInfo.test.tsx
      │   ├── UserActions/
      │   │   ├── UserActions.tsx
      │   │   └── UserActions.test.tsx
      │   └── UserSettings/
      │       ├── UserSettings.tsx
      │       └── UserSettings.test.tsx
    ```
  - **Changes**:
    1. Extract `UserInfo` component
    2. Extract `UserActions` component
    3. Extract `UserSettings` component
    4. Update `LargeComponent` to compose sub-components
    5. Update tests for each component
  - **Verification**: Visual regression testing, unit tests
  - **Estimated Time**: 45-60 minutes

### Phase 6: Testing and Validation
**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete

**Tasks**:
- [ ] Task 6.1: Write/update unit tests
  - **Files**: `**/*.test.ts`, `**/*.spec.ts`
  - **Action**: Ensure all refactored code has tests
  - **Coverage Target**: ≥80% coverage
- [ ] Task 6.2: Run test suite
  - **Action**: Execute all tests, verify all pass
- [ ] Task 6.3: Manual testing
  - **Action**: Test user workflows, verify no regressions
- [ ] Task 6.4: Performance validation
  - **Action**: Compare performance before/after
  - **Target**: No performance degradation

---

## Code Examples

### Example 1: Extract Method Refactoring

**Before**:
\`\`\`typescript
// File: src/services/UserService.ts
class UserService {
  processUser(userData: any): void {
    if (userData.type === 'admin') {
      // 20 lines of admin logic
      this.setAdminPermissions(userData);
      this.configureAdminUI(userData);
      this.setupAdminNotifications(userData);
      this.logAdminActivity(userData);
      // ... more admin logic
    } else if (userData.type === 'user') {
      // 20 lines of user logic
      this.setUserPermissions(userData);
      this.configureUserUI(userData);
      this.setupUserNotifications(userData);
      // ... more user logic
    }
  }
}
\`\`\`

**After**:
\`\`\`typescript
// File: src/services/UserService.ts
class UserService {
  processUser(userData: UserData): void {
    if (userData.type === 'admin') {
      this.processAdminUser(userData);
    } else if (userData.type === 'user') {
      this.processRegularUser(userData);
    }
  }
  
  private processAdminUser(userData: UserData): void {
    this.setAdminPermissions(userData);
    this.configureAdminUI(userData);
    this.setupAdminNotifications(userData);
    this.logAdminActivity(userData);
    // ... admin logic
  }
  
  private processRegularUser(userData: UserData): void {
    this.setUserPermissions(userData);
    this.configureUserUI(userData);
    this.setupUserNotifications(userData);
    // ... user logic
  }
}
\`\`\`

**Benefits**:
- ✅ Improved readability (clear method names)
- ✅ Single responsibility (each method does one thing)
- ✅ Easier to test (can test each method independently)
- ✅ Easier to maintain (changes isolated)

### Example 2: Extract Interface for Dependency Inversion

**Before**:
\`\`\`typescript
// File: src/services/UserService.ts
class UserService {
  private repository = new UserRepository();
  
  async getUser(id: string): Promise<User> {
    return this.repository.getUser(id);
  }
}
\`\`\`

**After**:
\`\`\`typescript
// File: src/types/IUserRepository.ts (new)
export interface IUserRepository {
  getUser(id: string): Promise<User>;
  saveUser(user: User): Promise<void>;
}

// File: src/repositories/UserRepository.ts
export class UserRepository implements IUserRepository {
  async getUser(id: string): Promise<User> {
    // Implementation
  }
  
  async saveUser(user: User): Promise<void> {
    // Implementation
  }
}

// File: src/services/UserService.ts
export class UserService {
  constructor(private repository: IUserRepository) {}
  
  async getUser(id: string): Promise<User> {
    return this.repository.getUser(id);
  }
}
\`\`\`

**Benefits**:
- ✅ Dependency inversion (depends on abstraction)
- ✅ Testability (can inject mock repository)
- ✅ Flexibility (can swap implementations)
- ✅ Single responsibility (service doesn't create dependencies)

---

## Testing Strategy

### Unit Testing

**Files to Test**:
- `src/services/UserService.test.ts` - Test refactored service
- `src/utils/validation.test.ts` - Test extracted utilities
- `src/components/UserInfo.test.tsx` - Test extracted components

**Test Cases**:
1. **UserService.processUser**:
   - Should call processAdminUser for admin users
   - Should call processRegularUser for regular users
   - Should handle invalid user types

2. **Validation utilities**:
   - Should validate email correctly
   - Should reject invalid emails
   - Should handle edge cases

3. **Component rendering**:
   - Should render user info correctly
   - Should handle loading states
   - Should handle error states

### Integration Testing

**Test Scenarios**:
1. User workflow: Create user → Process user → Validate user
2. Admin workflow: Create admin → Process admin → Configure admin

### Regression Testing

**Critical Paths to Test**:
1. User registration flow
2. User login flow
3. Admin dashboard
4. User settings

**Verification Checklist**:
- [ ] All existing tests pass
- [ ] No new test failures
- [ ] Performance maintained
- [ ] No visual regressions
- [ ] All user workflows work

---

## Risk Assessment

### High Risk Areas

#### Risk 1: Breaking Changes in UserService
**Probability**: Medium
**Impact**: High
**Mitigation**:
- Write characterization tests before refactoring
- Refactor incrementally (one method at a time)
- Run full test suite after each change
- Manual testing of critical workflows

#### Risk 2: Performance Degradation
**Probability**: Low
**Impact**: Medium
**Mitigation**:
- Performance testing before/after
- Profile critical paths
- Monitor bundle size
- Use performance budgets

### Rollback Strategy

**Git-Based Rollback**:
- Commit after each phase completion
- Use feature branch for refactoring
- Keep commits small and focused
- Tag stable points for easy rollback

**Feature Flag Rollback** (if applicable):
- Use feature flags for gradual rollout
- Monitor error rates
- Quick disable if issues detected

---

## Progress Tracking

### Refactoring Progress

| Phase | Status | Completed Tasks | Notes |
|-------|--------|-----------------|-------|
| Phase 1: Preparation | ⏸️ Not Started | 0/7 | Includes project-wide analysis |
| Phase 1.5: Component Decisions | ⏸️ Not Started | 0/2 | ⚠️ **USER INPUT REQUIRED** |
| Phase 2: Extract Abstractions | ⏸️ Not Started | 0/4 | Includes component extraction + theming |
| Phase 3: Decompose Functions | ⏸️ Not Started | 0/1 | Depends on Phase 2 |
| Phase 4: Improve Naming | ⏸️ Not Started | 0/1 | Can run in parallel |
| Phase 5: Component Decomposition | ⏸️ Not Started | 0/1 | Depends on Phase 3 |
| Phase 6: Testing | ⏸️ Not Started | 0/4 | Final phase |

**Legend**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete | ⚠️ Blocked | ⚠️ Awaiting User Input

### Active Work

**Currently Working On**: [None - Planning phase]

**Next Steps**:
1. Review this plan with team/stakeholders
2. Begin Phase 1: Preparation (includes project-wide component discovery)
3. Complete Phase 1.5: Get user decisions on component reuse/merge/create
4. Proceed to Phase 2 with documented decisions

---

## Handoff Checklist for Next AI

- [ ] Refactoring plan document created and complete
- [ ] **Project-wide context analysis completed** (entire codebase analyzed)
- [ ] **Theming system analyzed** (ThemedText, theme colors, M3 patterns identified)
- [ ] **Existing components discovered and documented** (including theming compliance)
- [ ] **Extractable components identified** with locations and rationale
- [ ] **Component reuse decisions obtained from user** (Phase 1.5 complete)
- [ ] **Theming migration requirements documented** for all components
- [ ] All code smells identified and documented
- [ ] Target state design is clear and detailed
- [ ] All refactoring steps have specific code examples
- [ ] Before/after code examples provided for major changes
- [ ] Component extraction tasks documented with user decisions
- [ ] Testing strategy defined
- [ ] Risk assessment completed
- [ ] Dependencies and impacts identified (project-wide)
- [ ] Estimated time for each task provided

**Next AI**: Start by reading this refactoring plan document first. **CRITICAL**: Ensure Phase 1.5 (Component Decisions) is complete with user input before proceeding to Phase 2. Begin with Phase 1, Task 1.1, and work through phases systematically. After each task, update progress tracking and verify no regressions. **Remember**: Think about the project as a whole, not just the target component.
```

---

## **File Naming Convention**

**Primary Location:** `planning/[ticket-id]/` or `planning/refactoring/` directory in workspace root

**Filename Pattern:** `[target-name].refactor-plan.md`

**Examples**:
- `planning/ref-12345/user-service-refactoring.refactor-plan.md`
- `planning/refactoring/component-decomposition.refactor-plan.md`
- `planning/bug-fix-123/legacy-code-cleanup.refactor-plan.md`

**Directory Structure**:
```
planning/
  ├── ref-12345/
  │   ├── user-service-refactoring.refactor-plan.md
  │   └── implementation-notes.md (optional)
  ├── refactoring/
  │   ├── component-decomposition.refactor-plan.md
  │   └── legacy-code-cleanup.refactor-plan.md
```

---

## **Usage Examples & When to Use**

### Required Usage

- **Before starting major refactoring** - Create detailed refactoring plan
- **When refactoring complex code** - Plan helps avoid breaking changes
- **When multiple files/components affected** - Coordinate refactoring across codebase
- **When refactoring legacy code** - Document current state and target state

### Recommended Usage

- **Before any refactoring** - Even small refactorings benefit from planning
- **When team collaboration needed** - Plan provides shared understanding
- **When refactoring critical paths** - Risk mitigation through planning

### Command Usage

```bash
/create-refactor-plan
```

**The command will**:
1. Analyze target code (read-only reconnaissance)
2. **Analyze entire codebase** for project-wide context and existing components
3. **Identify extractable components** that should become shared components
4. **Search for existing components** that could be reused, merged, or extended
5. **Request user decisions** on component reuse/merge/create options
6. Identify all refactoring opportunities
7. Create detailed refactoring plan with code examples
8. Document before/after code for each refactoring
9. Provide step-by-step execution plan
10. Include testing and risk assessment

---

## **Refactoring Plan Quality Requirements**

### Detail Level Requirements

**MUST Include**:
- ✅ **Project-wide context analysis** (entire codebase, not just target)
- ✅ **Theming system analysis** (ThemedText, theme colors, M3 patterns identified)
- ✅ **Existing component inventory** with similarity analysis and theming compliance
- ✅ **Extractable component identification** with locations and rationale
- ✅ **Theming compliance assessment** for all components (target + existing)
- ✅ **User decisions documented** for component reuse/merge/create
- ✅ **Theming migration requirements** documented for all components
- ✅ Specific file paths and line numbers for all changes
- ✅ Before/after code examples for each major refactoring (including theming changes)
- ✅ Step-by-step instructions for each task (including theming migration)
- ✅ Estimated time for each task
- ✅ Verification steps for each task (including theming compliance checks)
- ✅ Dependencies between tasks (including project-wide dependencies)
- ✅ Risk assessment and mitigation strategies

**VERY Detailed Means**:
- Not just "extract method" but showing exact code before/after
- Not just "improve naming" but showing old name → new name with rationale
- Not just "split component" but showing exact file structure and code changes
- Not just "extract component" but showing existing component search results, comparison, and user decision points
- Not just "add tests" but showing what to test and how
- Not just "analyze code" but analyzing the entire codebase for reuse opportunities
- Not just "apply theming" but showing exact Text→ThemedText, color→theme.colors.* migrations with code examples

---

## **Integration with Other Commands**

This command works with:
- `/refactor` - Execute refactoring based on this plan
- `/save-plan` - Update progress as refactoring proceeds
- `/save-context` - For supplementary refactoring context
- `/analyze-code-deep` - For comprehensive code analysis before planning

**Key Differences**:
- `/create-refactor-plan` → Creates detailed refactoring plan (planning phase)
- `/refactor` → Executes refactoring (execution phase)
- `/save-plan` → Tracks progress during execution

---

## **Success Criteria**

A well-created refactoring plan allows ANY AI to:

- [ ] Understand current code state completely (target + project-wide context)
- [ ] Understand theming system and compliance requirements (M3, ThemedText, theme colors)
- [ ] See existing components that could be reused or merged (including theming compliance)
- [ ] Understand component extraction opportunities and user decisions
- [ ] See exact target state with code examples (including theming migration examples)
- [ ] Execute refactoring step-by-step without ambiguity (including theming migration steps)
- [ ] Verify each step with clear criteria (including theming compliance checks)
- [ ] Understand risks and how to mitigate them (including project-wide risks and theming risks)
- [ ] Know what to test and how (including theming visual regression tests)
- [ ] Continue work seamlessly across sessions
- [ ] Make informed decisions about component reuse based on project-wide context
- [ ] Ensure all refactored components comply with theming system

**Remember**: The refactoring plan must be **VERY detailed** with specific code examples, before/after comparisons, and step-by-step instructions. The plan must think about the project as a whole, not just the target component. Component extraction and reuse decisions must be based on comprehensive project-wide analysis. **All refactored components MUST comply with the project's theming system (Material Design 3)**, using ThemedText, ThemedTextInput, theme colors, and M3 patterns.

