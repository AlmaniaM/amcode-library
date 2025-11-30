# Refactoring Protocol

## Overview

Execute systematic refactoring for any frontend application (React, Vue, Angular, Svelte, or vanilla JavaScript/TypeScript), focusing on code smell identification, safe refactoring execution, impact analysis, and validation with comprehensive refactoring strategies and best practices. **All refactoring MUST prioritize clean architecture, clean code principles, SOLID, DRY, YAGNI, testability, maintainability, and readability - with readability and maintainability as the primary goals.**

## Mission Briefing: Refactoring Protocol

**REFACTORING COMMAND DETECTION:** Before proceeding with refactoring, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's refactoring and execute the detected command instead to avoid duplicate analysis.

You will now execute systematic refactoring using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This refactoring follows the Phase 0 reconnaissance principles with specialized focus on code improvement, maintainability, and readability.

## **MANDATORY REFACTORING PRINCIPLES**

### Core Refactoring Philosophy

**Every refactoring MUST adhere to these principles in order of priority:**

1. **Readability First**: Code should be self-documenting and easy to understand
2. **Maintainability Second**: Code should be easy to modify and extend
3. **Testability**: Code should be easily testable in isolation
4. **Clean Architecture**: Proper separation of concerns and dependency management
5. **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
6. **DRY (Don't Repeat Yourself)**: Eliminate code duplication through proper abstraction
7. **YAGNI (You Aren't Gonna Need It)**: Avoid over-engineering and premature optimization

### **FORBIDDEN: Overengineering and Premature Abstraction**

**STRICTLY FORBIDDEN Patterns:**

- **FORBIDDEN**: Creating abstractions for single use cases (must have 3+ actual uses)
- **FORBIDDEN**: Adding complexity for "future needs" that don't exist yet
- **FORBIDDEN**: Implementing design patterns when simple code solves the problem
- **FORBIDDEN**: Creating interfaces/abstract classes for single implementations
- **FORBIDDEN**: Premature optimization without performance problems
- **FORBIDDEN**: Adding layers of indirection that don't improve readability
- **FORBIDDEN**: Creating "framework-like" abstractions for simple utilities
- **FORBIDDEN**: Implementing complex state management for simple state
- **FORBIDDEN**: Adding dependency injection containers for simple dependencies
- **FORBIDDEN**: Creating factory patterns for simple object creation

**YAGNI Validation Checklist (MANDATORY before creating abstractions):**

- [ ] **Current Need**: Does this abstraction solve a current, concrete problem?
- [ ] **Multiple Uses**: Do we have 3+ actual use cases, or just anticipated ones?
- [ ] **Simplicity First**: Can we solve this with simpler code first?
- [ ] **Future Proofing**: Are we adding complexity for future needs that may never come?
- [ ] **Readability Impact**: Does the abstraction make code easier to read, or harder?
- [ ] **Maintenance Cost**: Is the abstraction worth the maintenance overhead?

**If any answer is "No" or "Uncertain": DO NOT CREATE THE ABSTRACTION. Use simpler code.**

### Refactoring Quality Gates

**Before any refactoring is considered complete, verify:**

- [ ] **Readability**: Code reads like well-written prose
- [ ] **Maintainability**: Changes can be made without breaking other parts
- [ ] **Testability**: All code paths can be tested in isolation
- [ ] **SOLID Compliance**: Each class/function has single responsibility
- [ ] **DRY Compliance**: No duplicate logic exists
- [ ] **YAGNI Compliance**: No unnecessary abstractions or features (MANDATORY CHECK)
- [ ] **Clean Architecture**: Proper layer separation and dependency direction
- [ ] **Documentation**: Complex logic is documented with clear comments
- [ ] **No Overengineering**: Code is as simple as possible while meeting requirements

---

## **Phase 0: Refactoring Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of current code state to build a complete understanding of refactoring opportunities.

**Refactoring Analysis Scope:**

- **Code Smell Identification**: Long methods, large classes, duplicate code, tight coupling, poor naming, magic numbers/strings
- **Architecture Analysis**: Component/module structure, service organization, layer boundaries, dependency direction
- **Pattern Analysis**: Design patterns, framework patterns, anti-patterns, best practices
- **Dependency Analysis**: Service dependencies, component dependencies, circular dependencies, coupling metrics
- **Framework-Specific**: Framework-specific refactoring patterns and opportunities (React, Vue, Angular, Svelte, vanilla JS/TS)
- **Clean Code Violations**: Naming issues, function length, complexity, parameter count, return type clarity
- **SOLID Violations**: Single Responsibility violations, Open/Closed violations, Liskov Substitution issues, Interface Segregation problems, Dependency Inversion violations
- **DRY Violations**: Code duplication, repeated patterns, copy-paste code
- **YAGNI Violations**: Over-engineering, premature abstractions, unnecessary features
- **Testability Issues**: Hard-to-test code, tight coupling, lack of dependency injection, side effects
- **Readability Issues**: Complex logic, poor naming, missing documentation, unclear intent
- **Maintainability Issues**: Tight coupling, high complexity, unclear dependencies, fragile code

**Constraints:**

- **No mutations are permitted during this phase**
- **Focus solely on understanding current code state**
- **Identify refactoring opportunities without implementing changes**

---

## **Phase 1: Code Smell Identification**

**Directive:** Identify code smells and refactoring opportunities with focus on readability, maintainability, and clean code principles.

**Code Smell Analysis Areas:**

1. **Long Methods and Functions (Readability & Maintainability):**

   - Methods exceeding 20 lines (prefer 5-15 lines)
   - Functions with multiple responsibilities (SOLID violation)
   - Complex conditional logic (extract to named functions)
   - Nested loops and conditions (reduce nesting, extract methods)
   - Functions with too many parameters (extract objects, use configuration)
   - Functions that do too much (split into smaller, focused functions)

2. **Large Classes and Components (Single Responsibility):**

   - Components with excessive properties (split into smaller components)
   - Services with multiple responsibilities (extract to focused services)
   - Classes with too many methods (identify responsibilities, split)
   - Components with complex templates/JSX (extract sub-components)
   - Classes that violate Single Responsibility Principle

3. **Duplicate Code (DRY Principle):**

   - Repeated logic across components (extract to utilities/hooks/composables)
   - Duplicate template/JSX patterns (extract to reusable components)
   - Similar service methods (extract to base classes or utilities)
   - Repeated utility functions (consolidate into shared utilities)
   - Copy-paste code blocks (identify abstraction opportunities)

4. **Tight Coupling (Dependency Inversion & Maintainability):**

   - Direct component dependencies (use dependency injection)
   - Hard-coded service references (inject dependencies)
   - Tight module coupling (introduce interfaces/abstractions)
   - Circular dependencies (break cycles with dependency inversion)
   - Concrete dependencies instead of abstractions

5. **Poor Readability:**

   - Unclear variable/function names (rename for clarity)
   - Magic numbers and strings (extract to named constants)
   - Complex boolean expressions (extract to named functions)
   - Missing or unclear comments (add self-documenting code or comments)
   - Inconsistent code style (standardize formatting)

6. **Testability Issues:**

   - Hard-to-test code (introduce dependency injection)
   - Tight coupling to external systems (use mocks/interfaces)
   - Side effects in business logic (separate pure functions)
   - Lack of dependency injection (refactor to accept dependencies)
   - Global state dependencies (pass state explicitly)

7. **YAGNI Violations (CRITICAL - Overengineering Detection):**
   - **FORBIDDEN**: Over-engineered abstractions (simplify to current needs)
   - **FORBIDDEN**: Premature optimization (remove if not needed)
   - **FORBIDDEN**: Unnecessary features (remove unused code)
   - **FORBIDDEN**: Complex patterns for simple problems (use simpler solutions)
   - **FORBIDDEN**: Interfaces/abstract classes for single implementations
   - **FORBIDDEN**: Factory patterns for simple object creation
   - **FORBIDDEN**: Dependency injection containers for simple dependencies
   - **FORBIDDEN**: Complex state management for simple state
   - **FORBIDDEN**: "Framework-like" abstractions for simple utilities
   - **FORBIDDEN**: Layers of indirection that don't improve readability

**Overengineering Detection Checklist (MANDATORY):**
Before identifying any abstraction as a refactoring opportunity, verify:

- [ ] **Current Need**: Does this solve a current, concrete problem? (Not future need)
- [ ] **Multiple Uses**: Do we have 3+ actual use cases? (Not anticipated)
- [ ] **Simplicity Check**: Can simpler code solve this? (If yes, use simpler code)
- [ ] **Readability Impact**: Does abstraction improve readability? (If no, don't create)
- [ ] **Maintenance Cost**: Is abstraction worth maintenance overhead? (If no, don't create)

**If any answer is "No" or "Uncertain": DO NOT CREATE THE ABSTRACTION. This is overengineering.**

**Overengineering Examples to Detect:**

```typescript
// ❌ FORBIDDEN: Interface for single implementation (overengineering)
interface IEmailValidator {
  validate(email: string): boolean;
}
class EmailValidator implements IEmailValidator {
  validate(email: string): boolean {
    /* ... */
  }
}
// Only used once - interface is unnecessary

// ✅ CORRECT: Simple function (no overengineering)
function validateEmail(email: string): boolean {
  const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return EMAIL_REGEX.test(email);
}

// ❌ FORBIDDEN: Factory pattern for simple creation (overengineering)
class UserFactory {
  createUser(type: string): User {
    if (type === "admin") return new AdminUser();
    return new RegularUser();
  }
}
// Only used in one place - factory is unnecessary

// ✅ CORRECT: Simple creation (no overengineering)
function createUser(type: string): User {
  return type === "admin" ? new AdminUser() : new RegularUser();
}

// ❌ FORBIDDEN: Complex state management for simple state (overengineering)
class StateManager {
  private state: State;
  private subscribers: Subscriber[];
  subscribe(callback: Function): void {
    /* ... */
  }
  dispatch(action: Action): void {
    /* ... */
  }
}
// For simple component state - overengineering

// ✅ CORRECT: Simple state (no overengineering)
const [state, setState] = useState(initialState);
```

**Code Smell Identification Commands:**

```bash
# Code analysis
npx eslint . --ext .ts,.html
npx tslint --project tsconfig.json

# Complexity analysis
npx complexity-report src/
npx plato -r -d report src/

# Duplicate code detection
npx jscpd src/
npx duplicacy src/
```

**Code Smell Examples with Specific Locations:**

#### Example 1: Long Method (Code Smell)

**Location**: `src/services/UserService.ts:45-95`

**Current Code (Code Smell)**:

```typescript
// File: src/services/UserService.ts:45-95
processUserData(userData: any): void {
  // 50+ lines of complex logic
  if (userData.type === 'admin') {
    this.setAdminPermissions(userData);
    this.configureAdminUI(userData);
    this.setupAdminNotifications(userData);
    this.logAdminActivity(userData);
    this.sendAdminWelcomeEmail(userData);
    // ... 20+ more lines of admin logic
  } else if (userData.type === 'user') {
    this.setUserPermissions(userData);
    this.configureUserUI(userData);
    this.setupUserNotifications(userData);
    // ... 20+ more lines of user logic
  } else if (userData.type === 'guest') {
    this.setGuestPermissions(userData);
    this.configureGuestUI(userData);
    // ... 15+ more lines of guest logic
  }
  // More complex logic...
}
```

**Issues Identified**:

- [ ] Violates Single Responsibility (handles admin, user, and guest logic)
- [ ] Poor readability (unclear intent, too much nesting)
- [ ] Hard to test (tight coupling, multiple responsibilities)
- [ ] High complexity (cyclomatic complexity: 8, should be ≤5)
- [ ] Long function (50+ lines, should be ≤20)
- [ ] Uses `any` type (should use proper TypeScript types)

**Target Code (After Refactoring)**:

```typescript
// File: src/services/UserService.ts:45-52
processUserData(userData: UserData): void {
  if (userData.type === 'admin') {
    this.processAdminUser(userData);
  } else if (userData.type === 'user') {
    this.processRegularUser(userData);
  } else if (userData.type === 'guest') {
    this.processGuestUser(userData);
  }
}

// File: src/services/UserService.ts:54-65 (new methods)
private processAdminUser(userData: UserData): void {
  this.setAdminPermissions(userData);
  this.configureAdminUI(userData);
  this.setupAdminNotifications(userData);
  this.logAdminActivity(userData);
  this.sendAdminWelcomeEmail(userData);
  // ... admin logic
}

private processRegularUser(userData: UserData): void {
  this.setUserPermissions(userData);
  this.configureUserUI(userData);
  this.setupUserNotifications(userData);
  // ... user logic
}

private processGuestUser(userData: UserData): void {
  this.setGuestPermissions(userData);
  this.configureGuestUI(userData);
  // ... guest logic
}
```

**Refactoring Steps**:

1. Create characterization tests for current behavior
2. Extract `processAdminUser` method (lines 54-65)
3. Extract `processRegularUser` method (lines 67-75)
4. Extract `processGuestUser` method (lines 77-85)
5. Simplify main `processUserData` method (lines 45-52)
6. Replace `any` with `UserData` type
7. Run tests, verify behavior unchanged
8. Update tests to cover extracted methods

**Verification Checklist**:

- [ ] All existing tests pass
- [ ] New tests cover extracted methods
- [ ] Cyclomatic complexity reduced to ≤5
- [ ] Function length ≤20 lines
- [ ] No `any` types remain
- [ ] Code is more readable

**Estimated Time**: 25-30 minutes

#### Example 2: Duplicate Code (DRY Violation)

**Locations**:

- `src/components/UserForm.tsx:45-50`
- `src/components/AdminForm.tsx:38-43`
- `src/components/GuestForm.tsx:52-57`

**Current Code (Duplicated)**:

```typescript
// File: src/components/UserForm.tsx:45-50
function validateEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

// File: src/components/AdminForm.tsx:38-43 (DUPLICATE)
function validateEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}

// File: src/components/GuestForm.tsx:52-57 (DUPLICATE)
function validateEmail(email: string): boolean {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
}
```

**Target Code (After Refactoring)**:

```typescript
// File: src/utils/validation.ts (NEW FILE)
export function validateEmail(email: string): boolean {
  const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return EMAIL_REGEX.test(email);
}

// File: src/components/UserForm.tsx (UPDATED)
import { validateEmail } from "../../utils/validation";
// Remove local validateEmail function, use imported one

// File: src/components/AdminForm.tsx (UPDATED)
import { validateEmail } from "../../utils/validation";
// Remove local validateEmail function, use imported one

// File: src/components/GuestForm.tsx (UPDATED)
import { validateEmail } from "../../utils/validation";
// Remove local validateEmail function, use imported one
```

**Refactoring Steps**:

1. Create new file `src/utils/validation.ts`
2. Extract `validateEmail` function with improved naming (EMAIL_REGEX constant)
3. Export function
4. Update imports in UserForm.tsx, AdminForm.tsx, GuestForm.tsx
5. Remove duplicated code from all three files
6. Run tests, verify behavior unchanged

**Verification Checklist**:

- [ ] All existing tests pass
- [ ] No duplicate code remains
- [ ] Function is properly exported
- [ ] All imports updated correctly
- [ ] Code follows DRY principle

**Estimated Time**: 15-20 minutes

#### Example 3: Large Component (Single Responsibility Violation)

**Location**: `src/components/UserDashboard.tsx:1-350`

**Current Code (Code Smell)**:

```typescript
// File: src/components/UserDashboard.tsx:1-350
@Component({
  selector: "app-user-dashboard",
  template: `<!-- 200+ lines of template -->`,
})
export class UserDashboardComponent {
  // 20+ properties
  // 30+ methods
  // Multiple responsibilities:
  // - User data management
  // - UI state management
  // - Form validation
  // - API calls
  // - Notification handling
}
```

**Issues Identified**:

- [ ] Violates Single Responsibility (handles data, UI, validation, API, notifications)
- [ ] Large component (350+ lines, should be ≤200)
- [ ] Complex template (200+ lines, should be ≤100)
- [ ] Too many methods (30+ methods, should be ≤10)
- [ ] Hard to test (multiple responsibilities, tight coupling)
- [ ] Hard to maintain (changes affect multiple concerns)

**Target Structure (After Refactoring)**:

```
src/components/
  ├── UserDashboard/
  │   ├── UserDashboard.tsx (orchestrator, 50-80 lines)
  │   ├── UserDashboard.test.tsx
  │   ├── UserInfo/
  │   │   ├── UserInfo.tsx (50-80 lines)
  │   │   └── UserInfo.test.tsx
  │   ├── UserActions/
  │   │   ├── UserActions.tsx (50-80 lines)
  │   │   └── UserActions.test.tsx
  │   └── UserSettings/
  │       ├── UserSettings.tsx (50-80 lines)
  │       └── UserSettings.test.tsx
```

**Refactoring Steps**:

1. Create characterization tests for current behavior
2. Extract `UserInfo` component (handles user data display)
3. Extract `UserActions` component (handles user actions)
4. Extract `UserSettings` component (handles settings)
5. Update `UserDashboard` to compose sub-components
6. Update tests for each component
7. Run full test suite, verify no regressions

**Verification Checklist**:

- [ ] All existing tests pass
- [ ] Component size ≤200 lines
- [ ] Template size ≤100 lines
- [ ] Each component has single responsibility
- [ ] No visual regressions
- [ ] All user workflows work

**Estimated Time**: 45-60 minutes

---

## **Phase 2: Architecture Analysis**

**Directive:** Analyze application architecture and identify refactoring opportunities with focus on clean architecture, maintainability, and testability.

**Architecture Analysis Areas:**

1. **Component/Module Structure (Clean Architecture):**

   - Component hierarchy and organization (ensure proper layering)
   - Component responsibilities and boundaries (Single Responsibility)
   - Component communication patterns (prefer props/events over direct coupling)
   - Component reusability (extract reusable components)
   - Layer separation (presentation, business logic, data access)
   - Dependency direction (dependencies point inward, not outward)

2. **Service/Utility Organization (SOLID & Maintainability):**

   - Service responsibilities and boundaries (one responsibility per service)
   - Service dependencies and coupling (use dependency injection)
   - Service instantiation and lifecycle (manage dependencies properly)
   - Service testing and maintainability (testable in isolation)
   - Interface segregation (clients shouldn't depend on unused methods)
   - Dependency inversion (depend on abstractions, not concretions)

3. **Module Boundaries (Clean Architecture):**

   - Module organization and structure (clear boundaries)
   - Module dependencies and coupling (minimize coupling)
   - Module responsibilities and boundaries (single responsibility)
   - Module testing and maintainability (testable independently)
   - Dependency direction (inner layers don't depend on outer layers)

4. **Data Flow Analysis (Readability & Maintainability):**
   - Data flow patterns and organization (unidirectional flow preferred)
   - State management and updates (clear state management patterns)
   - Event handling and communication (explicit event contracts)
   - Data transformation and processing (pure functions where possible)
   - Side effect management (isolate side effects)
   - State mutation patterns (prefer immutability)

**Architecture Analysis Patterns:**

```typescript
// Poor component structure (refactoring needed)
@Component({
  selector: "app-user-management",
  template: `
    <div>
      <h1>User Management</h1>
      <div *ngFor="let user of users">
        <span>{{ user.name }}</span>
        <button (click)="editUser(user)">Edit</button>
        <button (click)="deleteUser(user)">Delete</button>
      </div>
      <form (ngSubmit)="addUser()">
        <input [(ngModel)]="newUser.name" placeholder="Name" />
        <input [(ngModel)]="newUser.email" placeholder="Email" />
        <button type="submit">Add User</button>
      </form>
    </div>
  `,
})
export class UserManagementComponent {
  users: User[] = [];
  newUser: User = { name: "", email: "" };

  // Multiple responsibilities mixed together
  loadUsers() {
    /* ... */
  }
  editUser(user: User) {
    /* ... */
  }
  deleteUser(user: User) {
    /* ... */
  }
  addUser() {
    /* ... */
  }
  validateUser(user: User) {
    /* ... */
  }
  saveUser(user: User) {
    /* ... */
  }
}

// Better component structure (refactored)
@Component({
  selector: "app-user-management",
  template: `
    <app-user-list
      [users]="users$ | async"
      (edit)="onEditUser($event)"
      (delete)="onDeleteUser($event)"
    >
    </app-user-list>
    <app-user-form (save)="onSaveUser($event)"> </app-user-form>
  `,
})
export class UserManagementComponent {
  users$ = this.userService.getUsers();

  constructor(private userService: UserService) {}

  onEditUser(user: User): void {
    this.userService.editUser(user);
  }

  onDeleteUser(user: User): void {
    this.userService.deleteUser(user);
  }

  onSaveUser(user: User): void {
    this.userService.saveUser(user);
  }
}
```

---

## **Phase 3: Refactoring Strategy Planning**

**Directive:** Plan systematic refactoring approach and strategy with emphasis on readability, maintainability, and clean code principles.

**Refactoring Strategy Areas:**

1. **Refactoring Prioritization (Readability & Maintainability First):**

   - Critical readability issues (unclear code, poor naming)
   - High-impact maintainability improvements (tight coupling, high complexity)
   - Testability blockers (hard-to-test code)
   - SOLID violations (multiple responsibilities, dependency issues)
   - DRY violations (code duplication)
   - YAGNI violations (over-engineering)
   - Medium-term architectural improvements (layer separation, dependency direction)
   - Long-term strategic refactoring (complete architecture overhaul if needed)

2. **Refactoring Techniques (Clean Code & SOLID):**

   - **Extract Method/Function**: Break down long methods into smaller, named functions
   - **Extract Class/Component**: Split large classes/components by responsibility
   - **Extract Variable**: Replace complex expressions with named variables
   - **Rename**: Improve naming for clarity and intent
   - **Move Method/Function**: Move methods to appropriate classes/modules
   - **Replace Conditional with Polymorphism**: Use strategy pattern for complex conditionals
   - **Introduce Parameter Object**: Reduce parameter count for readability
   - **Replace Magic Numbers/Strings**: Extract to named constants
   - **Extract Interface**: Create abstractions for dependency inversion
   - **Decompose Conditional**: Break complex conditionals into named functions
   - **Replace Nested Conditional with Guard Clauses**: Improve readability
   - **Introduce Null Object**: Handle null cases explicitly

3. **Safety Measures (Testability & Maintainability):**

   - Comprehensive test coverage (write tests before refactoring when possible)
   - Incremental refactoring approach (small, safe steps)
   - Rollback strategies (git commits, feature flags)
   - Validation and testing (verify behavior unchanged)
   - Refactoring in isolation (one concern at a time)

4. **Impact Analysis (Maintainability Focus):**
   - Component dependency analysis (identify coupling points)
   - Service impact assessment (understand dependencies)
   - Template/JSX and styling impact (visual regression testing)
   - User experience impact (no functional changes)
   - Test impact (update tests as needed)
   - Documentation impact (update docs for clarity)

**Refactoring Strategy Patterns:**

```typescript
// Extract Method refactoring
// Before
processUserData(userData: any): void {
  if (userData.type === 'admin') {
    // Complex admin logic
    this.setAdminPermissions(userData);
    this.configureAdminUI(userData);
    this.setupAdminNotifications(userData);
  } else if (userData.type === 'user') {
    // Complex user logic
    this.setUserPermissions(userData);
    this.configureUserUI(userData);
    this.setupUserNotifications(userData);
  }
}

// After
processUserData(userData: any): void {
  if (userData.type === 'admin') {
    this.processAdminUser(userData);
  } else if (userData.type === 'user') {
    this.processRegularUser(userData);
  }
}

private processAdminUser(userData: any): void {
  this.setAdminPermissions(userData);
  this.configureAdminUI(userData);
  this.setupAdminNotifications(userData);
}

private processRegularUser(userData: any): void {
  this.setUserPermissions(userData);
  this.configureUserUI(userData);
  this.setupUserNotifications(userData);
}
```

---

## **Phase 4: Safe Refactoring Execution**

**Directive:** Execute refactoring changes safely with comprehensive validation, prioritizing readability and maintainability improvements.

**CRITICAL BLOCKING CONDITIONS:**

- **MANDATORY**: Do NOT proceed to next phase if current phase quality gates fail
- **MANDATORY**: All tests MUST pass before moving to next refactoring step
- **MANDATORY**: Refactoring without tests is STRICTLY FORBIDDEN
- **MANDATORY**: YAGNI validation MUST pass before creating any abstraction
- **MANDATORY**: Each refactoring step MUST be verified before proceeding

**If Quality Gates Fail:**

1. **STOP** all refactoring work immediately
2. **ROLLBACK** to last stable commit
3. **FIX** issues before proceeding
4. **DOCUMENT** failure in refactoring log
5. **RE-RUN** quality gates before continuing

**Refactoring Execution Principles:**

1. **Readability First Execution:**

   - **MUST** improve naming (clear, descriptive names)
   - **MUST** extract complex logic to named functions
   - **MUST** add comments only when code can't be self-documenting
   - **MUST** simplify control flow (reduce nesting, use early returns)
   - **MUST** group related code together

2. **Maintainability Focus:**

   - **MUST** reduce coupling (introduce abstractions, dependency injection)
   - **MUST** improve cohesion (group related functionality)
   - **MUST** make code easier to modify (clear boundaries, single responsibility)
   - **MUST** improve extensibility (open/closed principle)
   - **MUST** document architectural decisions

3. **Incremental Refactoring (MANDATORY):**

   - **MUST** make small, focused changes (one improvement per commit)
   - **MUST** test after each change (continuous testing and validation)
   - **MUST** build incrementally (build on previous improvements)
   - **MUST** have rollback ready at each step (risk mitigation strategies)
   - **MUST** preserve behavior (refactoring doesn't change functionality)

4. **Test-Driven Refactoring (MANDATORY):**

   - **MUST** write tests before refactoring when possible (characterization tests)
   - **MUST** maintain test coverage during refactoring (don't reduce coverage)
   - **MUST** validate functionality after refactoring (all tests pass)
   - **MUST** ensure no regression issues (regression test suite)
   - **MUST** improve testability (make code easier to test)

5. **Refactoring Validation (MANDATORY):**

   - **MUST** validate functionality (behavior unchanged)
   - **MUST** validate readability (code is clearer)
   - **MUST** validate maintainability (easier to modify)
   - **MUST** validate testability (easier to test)
   - **MUST** validate code quality (SOLID, DRY, YAGNI compliance)
   - **MUST** validate performance (no performance degradation)

6. **YAGNI Validation (MANDATORY Before Creating Abstractions):**

   - **MUST** verify current need exists (not future need)
   - **MUST** verify 3+ actual use cases (not anticipated)
   - **MUST** verify simpler code doesn't solve problem
   - **MUST** verify abstraction improves readability
   - **FORBIDDEN** to create abstraction if any validation fails

7. **Rollback Strategies (MANDATORY):**
   - **MUST** commit frequently with small commits
   - **MUST** use feature flags for rollback (if applicable)
   - **MUST** have test-based validation (automated rollback triggers)
   - **MUST** document changes (clear commit messages)

**Safe Refactoring Execution Patterns with Detailed Steps:**

#### Task 4.1: Extract Method Refactoring (Example)

**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete | ⚠️ Blocked

**File**: `src/services/UserService.ts:45-95`

**Current Code**:

```typescript
// File: src/services/UserService.ts:45-95
processUserData(userData: any): void {
  if (userData.type === 'admin') {
    this.setAdminPermissions(userData);
    this.configureAdminUI(userData);
    this.setupAdminNotifications(userData);
    // ... 20+ more lines
  } else if (userData.type === 'user') {
    this.setUserPermissions(userData);
    this.configureUserUI(userData);
    this.setupUserNotifications(userData);
    // ... 20+ more lines
  }
}
```

**Target Code**:

```typescript
// File: src/services/UserService.ts:45-52
processUserData(userData: UserData): void {
  if (userData.type === 'admin') {
    this.processAdminUser(userData);
  } else if (userData.type === 'user') {
    this.processRegularUser(userData);
  }
}

// File: src/services/UserService.ts:54-75 (new methods)
private processAdminUser(userData: UserData): void {
  this.setAdminPermissions(userData);
  this.configureAdminUI(userData);
  this.setupAdminNotifications(userData);
  // ... admin logic
}

private processRegularUser(userData: UserData): void {
  this.setUserPermissions(userData);
  this.configureUserUI(userData);
  this.setupUserNotifications(userData);
  // ... user logic
}
```

**Step-by-Step Execution (MANDATORY)**:

1. **Write Characterization Tests** (MANDATORY):

   - **File**: `src/services/UserService.test.ts`
   - **Action**: Write tests that document current behavior
   - **Verification**: All tests pass before refactoring
   - **Estimated Time**: 10-15 minutes

2. **Extract `processAdminUser` Method**:

   - **File**: `src/services/UserService.ts:54-75`
   - **Action**:
     1. Create new private method `processAdminUser`
     2. Move admin logic to new method
     3. Replace admin logic in main method with call to new method
   - **Verification**: Run tests, verify behavior unchanged
   - **Estimated Time**: 5-10 minutes

3. **Extract `processRegularUser` Method**:

   - **File**: `src/services/UserService.ts:77-95`
   - **Action**:
     1. Create new private method `processRegularUser`
     2. Move user logic to new method
     3. Replace user logic in main method with call to new method
   - **Verification**: Run tests, verify behavior unchanged
   - **Estimated Time**: 5-10 minutes

4. **Replace `any` with Proper Type**:

   - **File**: `src/services/UserService.ts` (all occurrences)
   - **Action**:
     1. Create `UserData` interface if it doesn't exist
     2. Replace all `any` with `UserData` type
   - **Verification**: TypeScript compilation succeeds, tests pass
   - **Estimated Time**: 5 minutes

5. **Update Tests for Extracted Methods**:
   - **File**: `src/services/UserService.test.ts`
   - **Action**: Add tests for extracted methods
   - **Verification**: All tests pass, coverage maintained or improved
   - **Estimated Time**: 10-15 minutes

**Verification Checklist (MANDATORY)**:

- [ ] All existing tests pass
- [ ] New tests cover extracted methods
- [ ] Cyclomatic complexity reduced (target: ≤5)
- [ ] Function length ≤20 lines
- [ ] No `any` types remain
- [ ] Code is more readable
- [ ] Code is easier to maintain
- [ ] Code is easier to test
- [ ] YAGNI compliance verified (no overengineering)

**Blocking Conditions**:

- **CRITICAL**: Do NOT proceed to next refactoring if tests fail
- **CRITICAL**: Do NOT proceed if complexity is not reduced
- **CRITICAL**: Do NOT proceed if `any` types remain

**If Verification Fails**:

1. **STOP** immediately
2. **ROLLBACK** to previous commit
3. **FIX** issues
4. **RE-RUN** verification
5. **DOCUMENT** failure

**Total Estimated Time**: 35-55 minutes

#### Task 4.2: Extract Interface for Dependency Inversion (Example)

**Status**: ⏸️ Not Started | 🔄 In Progress | ✅ Complete | ⚠️ Blocked

**File**: `src/services/UserService.ts:10-15`

**Current Code**:

```typescript
// File: src/services/UserService.ts:10-15
class UserService {
  private repository = new UserRepository();

  async getUser(id: string): Promise<User> {
    return this.repository.getUser(id);
  }
}
```

**Target Code**:

```typescript
// File: src/types/IUserRepository.ts (NEW FILE)
export interface IUserRepository {
  getUser(id: string): Promise<User>;
  saveUser(user: User): Promise<void>;
}

// File: src/repositories/UserRepository.ts (UPDATED)
export class UserRepository implements IUserRepository {
  async getUser(id: string): Promise<User> {
    // Implementation
  }

  async saveUser(user: User): Promise<void> {
    // Implementation
  }
}

// File: src/services/UserService.ts (UPDATED)
export class UserService {
  constructor(private repository: IUserRepository) {}

  async getUser(id: string): Promise<User> {
    return this.repository.getUser(id);
  }
}
```

**Step-by-Step Execution (MANDATORY)**:

1. **Create Interface**:

   - **File**: `src/types/IUserRepository.ts` (new)
   - **Action**: Create interface with required methods
   - **Verification**: TypeScript compilation succeeds
   - **Estimated Time**: 5 minutes

2. **Update Repository to Implement Interface**:

   - **File**: `src/repositories/UserRepository.ts`
   - **Action**: Add `implements IUserRepository`
   - **Verification**: TypeScript compilation succeeds
   - **Estimated Time**: 2 minutes

3. **Update Service to Accept Interface**:

   - **File**: `src/services/UserService.ts:10-15`
   - **Action**:
     1. Change constructor to accept `IUserRepository`
     2. Remove direct instantiation
   - **Verification**: TypeScript compilation succeeds
   - **Estimated Time**: 5 minutes

4. **Update All Instantiations**:

   - **Files**: All files that create `UserService`
   - **Action**: Inject `IUserRepository` in constructor
   - **Verification**: All instantiations updated, tests pass
   - **Estimated Time**: 10-15 minutes

5. **Update Tests with Mocks**:
   - **File**: `src/services/UserService.test.ts`
   - **Action**: Update tests to use mock `IUserRepository`
   - **Verification**: All tests pass
   - **Estimated Time**: 10-15 minutes

**Verification Checklist (MANDATORY)**:

- [ ] Interface created and exported
- [ ] Repository implements interface
- [ ] Service accepts interface in constructor
- [ ] All instantiations updated
- [ ] All tests pass with mocks
- [ ] Dependency inversion achieved
- [ ] Testability improved
- [ ] YAGNI compliance verified (interface has actual use cases)

**Blocking Conditions**:

- **CRITICAL**: Do NOT proceed if interface has unused methods (Interface Segregation violation)
- **CRITICAL**: Do NOT proceed if tests don't pass
- **CRITICAL**: Do NOT proceed if all instantiations aren't updated

**Total Estimated Time**: 32-42 minutes

**Refactoring Execution Commands:**

```bash
# Test execution
npm run test
npm run test:watch
npm run test:coverage

# Refactoring validation
npm run validate:refactoring
npm run validate:functionality
npm run validate:performance

# Rollback strategies
git checkout previous-commit
git revert refactoring-commit
git reset --hard HEAD~1
```

---

## **Phase 5: Framework-Specific Refactoring**

**Directive:** Execute framework-specific refactoring patterns and improvements while maintaining clean code principles and readability.

**Framework-Specific Refactoring Areas:**

### React Refactoring:

1. **Component Refactoring:**

   - Component decomposition (extract smaller components)
   - JSX refactoring (extract sub-components, improve readability)
   - Props optimization (use proper prop types, avoid prop drilling)
   - Hook extraction (custom hooks for reusable logic)
   - Component lifecycle optimization (useEffect, useMemo, useCallback appropriately)

2. **State Management:**

   - State decomposition (split complex state)
   - Custom hooks extraction (reusable state logic)
   - Context optimization (avoid unnecessary re-renders)
   - State management patterns (useReducer for complex state)

3. **Performance Optimization:**
   - Memoization (React.memo, useMemo, useCallback)
   - Code splitting (lazy loading components)
   - Bundle optimization (tree shaking, dynamic imports)

### Vue Refactoring:

1. **Component Refactoring:**

   - Component decomposition (extract smaller components)
   - Template refactoring (extract sub-components)
   - Composition API optimization (use composables for logic)
   - Props optimization (proper prop definitions)

2. **Composables Extraction:**
   - Extract reusable logic to composables
   - Separate concerns (data, computed, methods)
   - Improve testability (composables are easier to test)

### Angular Refactoring:

1. **Component Refactoring:**

   - Component decomposition (extract smaller components)
   - Template refactoring (extract sub-components)
   - Component communication optimization (use services, not direct coupling)
   - Component lifecycle optimization (OnPush change detection)

2. **Service Refactoring:**

   - Service decomposition (single responsibility)
   - Dependency injection optimization (proper DI patterns)
   - Observable pattern optimization (proper RxJS usage)
   - Error handling improvement (consistent error handling)

3. **Module Refactoring:**
   - Module organization (feature modules)
   - Module boundary optimization (clear boundaries)
   - Lazy loading optimization (route-based code splitting)
   - Module dependency management (avoid circular dependencies)

### Universal Refactoring (All Frameworks):

1. **Code Organization:**

   - File structure (clear organization)
   - Import organization (grouped, sorted imports)
   - Naming conventions (consistent, clear names)

2. **Error Handling:**

   - Consistent error handling patterns
   - Error boundaries (React) / Error handling (Vue/Angular)
   - User-friendly error messages

3. **Type Safety:**
   - TypeScript optimization (proper types, avoid `any`)
   - Type definitions (clear interfaces/types)
   - Type inference (let TypeScript infer when possible)

**Angular Refactoring Patterns:**

```typescript
// Component decomposition
// Before: Large component
@Component({
  selector: "app-user-dashboard",
  template: `
    <div>
      <h1>User Dashboard</h1>
      <div class="user-info">
        <h2>User Information</h2>
        <p>{{ user.name }}</p>
        <p>{{ user.email }}</p>
      </div>
      <div class="user-actions">
        <h2>Actions</h2>
        <button (click)="editProfile()">Edit Profile</button>
        <button (click)="changePassword()">Change Password</button>
      </div>
      <div class="user-settings">
        <h2>Settings</h2>
        <form (ngSubmit)="updateSettings()">
          <!-- Complex form -->
        </form>
      </div>
    </div>
  `,
})
export class UserDashboardComponent {
  // Multiple responsibilities
}

// After: Decomposed components
@Component({
  selector: "app-user-dashboard",
  template: `
    <div>
      <h1>User Dashboard</h1>
      <app-user-info [user]="user"></app-user-info>
      <app-user-actions
        (editProfile)="onEditProfile()"
        (changePassword)="onChangePassword()"
      ></app-user-actions>
      <app-user-settings
        (updateSettings)="onUpdateSettings($event)"
      ></app-user-settings>
    </div>
  `,
})
export class UserDashboardComponent {
  user: User;

  onEditProfile(): void {
    /* ... */
  }
  onChangePassword(): void {
    /* ... */
  }
  onUpdateSettings(settings: any): void {
    /* ... */
  }
}
```

---

## **Phase 6: Refactoring Validation and Testing**

**Directive:** Validate refactoring results and ensure quality. **MANDATORY**: All validation gates MUST pass before refactoring is considered complete.

**CRITICAL BLOCKING CONDITIONS:**

- **MANDATORY**: Do NOT mark refactoring as complete if any validation gate fails
- **MANDATORY**: All tests MUST pass (100% of existing tests + new tests)
- **MANDATORY**: YAGNI validation MUST pass (no overengineering detected)
- **MANDATORY**: Performance MUST not degrade (within 5% tolerance)
- **MANDATORY**: Code quality metrics MUST improve or maintain

**If Validation Fails:**

1. **STOP** and document failure
2. **ROLLBACK** to last stable state
3. **FIX** issues
4. **RE-RUN** all validation gates
5. **DO NOT** proceed until all gates pass

**Refactoring Validation Areas:**

1. **Functionality Validation (MANDATORY):**

   - **MUST** test feature functionality (all features work)
   - **MUST** test user workflows (critical paths work)
   - **MUST** run integration tests (components work together)
   - **MUST** run regression tests (no regressions)
   - **Verification Checklist**:
     - [ ] All existing tests pass
     - [ ] No new test failures
     - [ ] All user workflows work
     - [ ] No functional regressions
     - [ ] Integration tests pass

2. **Performance Validation (MANDATORY):**

   - **MUST** assess performance impact (within 5% tolerance)
   - **MUST** analyze bundle size (no significant increase)
   - **MUST** test runtime performance (no degradation)
   - **MUST** validate memory usage (no leaks)
   - **Verification Checklist**:
     - [ ] Performance within 5% of baseline
     - [ ] Bundle size not significantly increased
     - [ ] Runtime performance maintained
     - [ ] No memory leaks detected
     - [ ] Performance metrics documented

3. **Code Quality Validation (MANDATORY):**

   - **MUST** check code quality metrics (improved or maintained)
   - **MUST** analyze complexity (reduced or maintained)
   - **MUST** detect duplicate code (no new duplication)
   - **MUST** assess maintainability (improved)
   - **MUST** verify YAGNI compliance (no overengineering)
   - **Verification Checklist**:
     - [ ] Code quality metrics improved or maintained
     - [ ] Cyclomatic complexity reduced or maintained
     - [ ] No new code duplication
     - [ ] Maintainability improved
     - [ ] YAGNI compliance verified (no unnecessary abstractions)
     - [ ] SOLID principles followed
     - [ ] DRY principles followed

4. **YAGNI Compliance Validation (MANDATORY):**

   - **MUST** verify no overengineering (check all abstractions)
   - **MUST** verify abstractions have 3+ actual uses (not anticipated)
   - **MUST** verify simpler code doesn't solve problem
   - **MUST** verify abstraction improves readability
   - **Verification Checklist**:
     - [ ] All abstractions have current, concrete need
     - [ ] All abstractions have 3+ actual use cases
     - [ ] Simpler code doesn't solve the problem
     - [ ] Abstractions improve readability
     - [ ] No "future-proofing" abstractions
     - [ ] No premature optimizations
     - [ ] No unnecessary design patterns

5. **User Experience Validation (MANDATORY):**
   - **MUST** test user interface (UI works correctly)
   - **MUST** test accessibility (WCAG compliance)
   - **MUST** test cross-browser (all target browsers)
   - **MUST** test mobile responsiveness (mobile devices)
   - **Verification Checklist**:
     - [ ] UI works correctly
     - [ ] Accessibility maintained or improved
     - [ ] Cross-browser compatibility maintained
     - [ ] Mobile responsiveness maintained
     - [ ] No visual regressions

**Refactoring Validation Commands:**

```bash
# Functionality validation
npm run test
npm run test:e2e
npm run test:integration

# Performance validation
npm run test:performance
npm run build --stats-json
npx webpack-bundle-analyzer dist/stats.json

# Code quality validation
npm run lint
npm run test:coverage
npx complexity-report src/

# User experience validation
npm run test:e2e
npm run test:accessibility
npm run test:cross-browser
```

---

## **Phase 7: Refactoring Documentation and Maintenance**

**Directive:** Document refactoring changes and establish maintenance practices.

**Refactoring Documentation Areas:**

1. **Refactoring Documentation:**

   - Refactoring decisions and rationale
   - Code changes and improvements
   - Performance impact documentation
   - User experience improvements

2. **Maintenance Guidelines:**

   - Code quality standards
   - Refactoring best practices
   - Testing requirements
   - Performance monitoring

3. **Team Training:**

   - Refactoring techniques training
   - Code quality awareness
   - Testing practices training
   - Performance optimization training

4. **Continuous Improvement:**
   - Regular code reviews
   - Refactoring opportunities identification
   - Performance monitoring
   - Quality metrics tracking

**Refactoring Documentation Patterns:**

```typescript
// Refactoring documentation
/**
 * REFACTORING: UserService decomposition
 *
 * Rationale:
 * - Original UserService had multiple responsibilities
 * - Decomposed into focused services for better maintainability
 * - Improved testability and code organization
 *
 * Changes:
 * - Extracted UserDataService for data operations
 * - Extracted UserValidationService for validation logic
 * - Extracted UserNotificationService for notifications
 *
 * Performance Impact:
 * - Reduced service instantiation overhead
 * - Improved memory usage through focused services
 * - Enhanced testability and maintainability
 *
 * Breaking Changes:
 * - UserService constructor parameters changed
 * - Some methods moved to specialized services
 * - Updated dependency injection configuration
 */
@Injectable()
export class UserService {
  // Refactored service implementation
}
```

---

## **Usage Examples**

- `/refactor` - Execute comprehensive refactoring analysis
- `/refactor --smell-only` - Focus on code smell identification only
- `/refactor --architecture-only` - Focus on architecture analysis only
- `/refactor --framework-only` - Focus on framework-specific refactoring only
- `/refactor @src/components/` - Refactor specific component directory
- `/refactor --safe-execution` - Execute safe refactoring with validation
- `/refactor --validation-only` - Focus on refactoring validation only
- `/refactor --documentation-only` - Focus on refactoring documentation only

## **Refactoring Features**

- **Code Smell Identification**: Comprehensive code smell detection and analysis
- **Architecture Analysis**: Application architecture and structure analysis with clean architecture principles
- **Safe Refactoring**: Incremental, test-driven refactoring execution prioritizing readability and maintainability
- **Framework-Specific**: Framework-specific refactoring patterns and techniques (React, Vue, Angular, Svelte)
- **Clean Code Principles**: SOLID, DRY, YAGNI compliance with emphasis on readability and maintainability
- **Testability**: Refactoring to improve testability and maintain comprehensive test coverage
- **Validation and Testing**: Comprehensive refactoring validation and testing
- **Documentation**: Refactoring documentation and maintenance guidelines
- **Performance Impact**: Performance analysis and optimization during refactoring (without sacrificing readability)

## **CRITICAL ENFORCEMENT SUMMARY**

**MANDATORY Requirements:**

- **MUST** follow all blocking conditions (do not proceed if gates fail)
- **MUST** verify YAGNI compliance before creating any abstraction
- **MUST** pass all validation gates before marking refactoring complete
- **MUST** write tests before refactoring (characterization tests)
- **MUST** maintain or improve test coverage
- **MUST** verify no overengineering (check all abstractions)
- **MUST** preserve behavior (refactoring doesn't change functionality)

**FORBIDDEN Practices:**

- **FORBIDDEN**: Refactoring without tests
- **FORBIDDEN**: Creating abstractions for single use cases
- **FORBIDDEN**: Overengineering and premature abstraction
- **FORBIDDEN**: Proceeding if quality gates fail
- **FORBIDDEN**: Proceeding if tests fail
- **FORBIDDEN**: Proceeding if YAGNI validation fails
- **FORBIDDEN**: Creating abstractions for "future needs"
- **FORBIDDEN**: Adding complexity that doesn't improve readability

**Blocking Conditions (Do NOT Proceed If):**

- ⚠️ Quality gates fail
- ⚠️ Tests fail
- ⚠️ YAGNI validation fails
- ⚠️ Performance degrades beyond 5% tolerance
- ⚠️ Code quality metrics decrease
- ⚠️ Overengineering detected

**Remember**: This command provides comprehensive refactoring capabilities to improve code quality, maintainability, and readability through systematic refactoring strategies and best practices. **Readability and maintainability are the primary goals of all refactoring efforts. Overengineering is STRICTLY FORBIDDEN. All blocking conditions MUST be respected.**
