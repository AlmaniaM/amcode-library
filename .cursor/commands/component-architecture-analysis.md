# Component Architecture Analysis Protocol

## Overview
Execute deep architecture analysis of Angular components, focusing on component design patterns, Angular Style Guide compliance, project-specific patterns, and architectural best practices.

## Mission Briefing: Component Architecture Analysis Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute deep component architecture analysis using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows the Phase 0 reconnaissance principles with specialized focus on Angular component architecture and design patterns.

---

## **Phase 0: Component Architecture Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of Angular component files (.ts and .html) to build a complete understanding of current architecture patterns.

**Component Architecture Analysis Scope:**
- **Component Design Patterns**: Smart/dumb components, container/presentational patterns
- **Angular Style Guide Compliance**: Naming conventions, file organization, component structure
- **Project-Specific Patterns**: Existing project patterns, conventions, and standards
- **Linting and Rules Compliance**: ESLint rules, project-specific linting standards
- **Architecture Best Practices**: SOLID principles, separation of concerns, maintainability
- **Component Relationships**: Parent-child communication, service integration, dependency management

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus solely on understanding current component architecture state**
- **Analyze both .ts and .html files comprehensively**

---

## **Phase 1: Component Design Pattern Analysis**

**Directive:** Analyze component design patterns and architectural decisions.

**Design Pattern Analysis Areas:**

1. **Smart vs Dumb Components:**
   - Component responsibility analysis
   - State management patterns
   - Data flow patterns
   - Component reusability

2. **Container/Presentational Pattern:**
   - Container component identification
   - Presentational component identification
   - Data flow between components
   - Event handling patterns

3. **Component Composition:**
   - Component hierarchy analysis
   - Component composition patterns
   - Component communication patterns
   - Component isolation

4. **Service Integration:**
   - Service dependency patterns
   - Service injection patterns
   - Service usage patterns
   - Service testing patterns

**Design Pattern Analysis Patterns:**
```typescript
// Smart Component Analysis
@Component({
  selector: 'app-user-list-container', // ✅ Good: Container naming
  template: `
    <app-user-list 
      [users]="users$ | async"
      [loading]="loading$ | async"
      (userSelected)="onUserSelected($event)"
      (userDeleted)="onUserDeleted($event)">
    </app-user-list>
  `
})
export class UserListContainerComponent {
  users$ = this.userService.getUsers();
  loading$ = this.userService.getLoadingState();
  
  constructor(private userService: UserService) {}
  
  onUserSelected(user: User): void {
    this.router.navigate(['/users', user.id]);
  }
  
  onUserDeleted(user: User): void {
    this.userService.deleteUser(user.id);
  }
}

// Presentational Component Analysis
@Component({
  selector: 'app-user-list', // ✅ Good: Presentational naming
  template: `
    <div *ngIf="loading" class="loading">Loading...</div>
    <div *ngFor="let user of users" class="user-item">
      <span>{{ user.name }}</span>
      <button (click)="selectUser(user)">Select</button>
      <button (click)="deleteUser(user)">Delete</button>
    </div>
  `
})
export class UserListComponent {
  @Input() users: User[] = [];
  @Input() loading = false;
  @Output() userSelected = new EventEmitter<User>();
  @Output() userDeleted = new EventEmitter<User>();
  
  selectUser(user: User): void {
    this.userSelected.emit(user);
  }
  
  deleteUser(user: User): void {
    this.userDeleted.emit(user);
  }
}
```

---

## **Phase 2: Angular Style Guide Compliance Analysis**

**Directive:** Analyze compliance with Angular Style Guide and best practices.

**Style Guide Analysis Areas:**

1. **Naming Conventions:**
   - Component naming patterns
   - File naming conventions
   - Selector naming patterns
   - Property and method naming

2. **File Organization:**
   - File structure and organization
   - Import organization
   - Export patterns
   - Module organization

3. **Component Structure:**
   - Component decorator usage
   - Input/Output property patterns
   - Lifecycle hook implementation
   - Template organization

4. **Service Patterns:**
   - Service naming conventions
   - Service structure and organization
   - Service injection patterns
   - Service testing patterns

**Style Guide Analysis Patterns:**
```typescript
// Angular Style Guide Compliance Analysis
@Component({
  selector: 'app-user-profile', // ✅ Good: kebab-case selector
  templateUrl: './user-profile.component.html', // ✅ Good: Separate template file
  styleUrls: ['./user-profile.component.scss'], // ✅ Good: Separate style files
  changeDetection: ChangeDetectionStrategy.OnPush // ✅ Good: OnPush strategy
})
export class UserProfileComponent implements OnInit, OnDestroy { // ✅ Good: Interface implementation
  // ✅ Good: Input properties first
  @Input() userId: string = '';
  @Input() readonly = false;
  
  // ✅ Good: Output properties after inputs
  @Output() profileUpdated = new EventEmitter<User>();
  @Output() profileDeleted = new EventEmitter<string>();
  
  // ✅ Good: Public properties
  user: User | null = null;
  loading = false;
  
  // ✅ Good: Private properties
  private destroy$ = new Subject<void>();
  
  // ✅ Good: Constructor with dependency injection
  constructor(
    private userService: UserService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}
  
  // ✅ Good: Lifecycle hooks
  ngOnInit(): void {
    this.loadUser();
  }
  
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  // ✅ Good: Public methods
  updateProfile(user: User): void {
    this.userService.updateUser(user)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.profileUpdated.emit(user);
        this.cdr.markForCheck();
      });
  }
  
  // ✅ Good: Private methods
  private loadUser(): void {
    this.loading = true;
    this.userService.getUser(this.userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(user => {
        this.user = user;
        this.loading = false;
        this.cdr.markForCheck();
      });
  }
}
```

---

## **Phase 3: Project-Specific Pattern Analysis**

**Directive:** Analyze compliance with existing project patterns and conventions.

**Project Pattern Analysis Areas:**

1. **Existing Component Patterns:**
   - Component structure patterns
   - Component naming conventions
   - Component organization patterns
   - Component communication patterns

2. **Service Integration Patterns:**
   - Service usage patterns
   - Service injection patterns
   - Service testing patterns
   - Service error handling patterns

3. **Template Patterns:**
   - Template structure patterns
   - Template naming conventions
   - Template organization patterns
   - Template accessibility patterns

4. **Module Organization:**
   - Module structure patterns
   - Module naming conventions
   - Module organization patterns
   - Module dependency patterns

**Project Pattern Analysis Commands:**
```bash
# Project pattern analysis
find src -name "*.component.ts" -exec grep -l "export class" {} \;
find src -name "*.component.html" -exec wc -l {} \;
find src -name "*.service.ts" -exec grep -l "export class" {} \;

# Pattern consistency analysis
grep -r "selector:" src --include="*.component.ts"
grep -r "templateUrl:" src --include="*.component.ts"
grep -r "styleUrls:" src --include="*.component.ts"
```

---

## **Phase 4: Linting and Rules Compliance Analysis**

**Directive:** Analyze compliance with project linting rules and development standards.

**Linting Analysis Areas:**

1. **ESLint Rule Compliance:**
   - Angular-specific ESLint rules
   - TypeScript ESLint rules
   - Custom project rules
   - Rule violation analysis

2. **Code Style Compliance:**
   - Prettier formatting compliance
   - Indentation and spacing
   - Line length compliance
   - Import organization

3. **TypeScript Compliance:**
   - Type safety compliance
   - Interface usage patterns
   - Generic type usage
   - Strict mode compliance

4. **Angular-Specific Rules:**
   - Component naming rules
   - Service naming rules
   - Module organization rules
   - Template rules

**Linting Analysis Commands:**
```bash
# ESLint analysis
npx eslint src --ext .ts,.html --format=json

# TypeScript compliance
npx tsc --noEmit --strict

# Code style analysis
npx prettier --check src
npx stylelint "src/**/*.scss"
```

---

## **Phase 5: Architecture Best Practices Analysis**

**Directive:** Analyze compliance with architectural best practices and SOLID principles.

**Architecture Analysis Areas:**

1. **Single Responsibility Principle:**
   - Component responsibility analysis
   - Service responsibility analysis
   - Method responsibility analysis
   - Class responsibility analysis

2. **Open/Closed Principle:**
   - Component extensibility
   - Service extensibility
   - Interface usage patterns
   - Abstraction patterns

3. **Liskov Substitution Principle:**
   - Interface implementation
   - Inheritance patterns
   - Polymorphism usage
   - Contract compliance

4. **Interface Segregation Principle:**
   - Interface design patterns
   - Interface usage patterns
   - Interface dependency patterns
   - Interface optimization

5. **Dependency Inversion Principle:**
   - Dependency injection patterns
   - Service abstraction patterns
   - Interface dependency patterns
   - Inversion of control patterns

**Architecture Analysis Patterns:**
```typescript
// SOLID Principles Analysis
// ✅ Good: Single Responsibility Principle
@Component({
  selector: 'app-user-display',
  template: `<div>{{ user.name }}</div>`
})
export class UserDisplayComponent {
  @Input() user: User = {} as User;
  // Only responsible for displaying user data
}

// ✅ Good: Open/Closed Principle
interface UserRepository {
  getUser(id: string): Observable<User>;
  saveUser(user: User): Observable<User>;
}

@Injectable()
export class ApiUserRepository implements UserRepository {
  constructor(private http: HttpClient) {}
  
  getUser(id: string): Observable<User> {
    return this.http.get<User>(`/api/users/${id}`);
  }
  
  saveUser(user: User): Observable<User> {
    return this.http.post<User>('/api/users', user);
  }
}

// ✅ Good: Dependency Inversion Principle
@Injectable()
export class UserService {
  constructor(private userRepository: UserRepository) {}
  
  getUser(id: string): Observable<User> {
    return this.userRepository.getUser(id);
  }
}
```

---

## **Phase 6: Component Relationship Analysis**

**Directive:** Analyze component relationships and communication patterns.

**Relationship Analysis Areas:**

1. **Parent-Child Communication:**
   - Input property patterns
   - Output event patterns
   - Event handling patterns
   - Data flow patterns

2. **Service Integration:**
   - Service dependency patterns
   - Service injection patterns
   - Service usage patterns
   - Service testing patterns

3. **Component Dependencies:**
   - Component dependency analysis
   - Circular dependency detection
   - Dependency optimization
   - Dependency testing

4. **Module Dependencies:**
   - Module dependency analysis
   - Module organization patterns
   - Module testing patterns
   - Module optimization

**Relationship Analysis Patterns:**
```typescript
// Component Relationship Analysis
@Component({
  selector: 'app-parent',
  template: `
    <app-child 
      [data]="data"
      [config]="config"
      (dataChanged)="onDataChanged($event)"
      (configChanged)="onConfigChanged($event)">
    </app-child>
  `
})
export class ParentComponent {
  data = signal<any[]>([]);
  config = signal<Config>({} as Config);
  
  onDataChanged(newData: any[]): void {
    this.data.set(newData);
  }
  
  onConfigChanged(newConfig: Config): void {
    this.config.set(newConfig);
  }
}

@Component({
  selector: 'app-child',
  template: `
    <div *ngFor="let item of data()">
      {{ item.name }}
    </div>
  `
})
export class ChildComponent {
  @Input() data = signal<any[]>([]);
  @Input() config = signal<Config>({} as Config);
  @Output() dataChanged = new EventEmitter<any[]>();
  @Output() configChanged = new EventEmitter<Config>();
  
  updateData(newData: any[]): void {
    this.dataChanged.emit(newData);
  }
}
```

---

## **Phase 7: Architecture Recommendations and Prioritization**

**Directive:** Synthesize analysis into prioritized architecture recommendations.

**Recommendation Categories:**

1. **Critical Issues (High Priority):**
   - SOLID principle violations
   - Angular Style Guide violations
   - Linting rule violations
   - Architecture anti-patterns

2. **High Priority Improvements:**
   - Component responsibility optimization
   - Service integration improvements
   - Template organization improvements
   - Dependency management optimization

3. **Medium Priority Enhancements:**
   - Code organization improvements
   - Naming convention standardization
   - Documentation enhancements
   - Testing pattern improvements

4. **Low Priority Optimizations:**
   - Performance optimizations
   - Code style improvements
   - Future architecture considerations
   - Monitoring and maintenance improvements

**Architecture Recommendations Output:**
```markdown
## Component Architecture Analysis Report

### Critical Issues (Fix Immediately)
1. **Single Responsibility Violation** - HIGH IMPACT
   - Location: UserManagementComponent
   - Issue: Component handles both user display and user editing
   - Solution: Split into UserDisplayComponent and UserEditComponent
   - Priority: CRITICAL

2. **Angular Style Guide Violation** - HIGH IMPACT
   - Location: user-profile.component.ts
   - Issue: Missing interface implementation for lifecycle hooks
   - Solution: Implement OnInit, OnDestroy interfaces
   - Priority: CRITICAL

### High Priority Improvements
1. **Service Integration Pattern** - MEDIUM IMPACT
   - Location: Component constructor
   - Issue: Direct service injection without abstraction
   - Solution: Use interface-based dependency injection
   - Priority: HIGH

2. **Template Organization** - MEDIUM IMPACT
   - Location: Component template
   - Issue: Template exceeds 50 lines
   - Solution: Extract into smaller components
   - Priority: HIGH

### Medium Priority Enhancements
1. **Naming Convention Standardization** - LOW IMPACT
   - Location: Component selectors
   - Issue: Inconsistent selector naming
   - Solution: Standardize to kebab-case
   - Priority: MEDIUM

2. **Code Organization** - LOW IMPACT
   - Location: Component file structure
   - Issue: Methods not organized by visibility
   - Solution: Organize public, private methods
   - Priority: MEDIUM
```

---

## **Usage Examples**

- `/component-architecture-analysis @src/components/user-profile.component.ts` - Analyze specific component
- `/component-architecture-analysis @src/components/` - Analyze component directory
- `/component-architecture-analysis --style-guide-only` - Focus on Angular Style Guide compliance
- `/component-architecture-analysis --solid-principles-only` - Focus on SOLID principles analysis
- `/component-architecture-analysis --linting-only` - Focus on linting compliance analysis

## **Component Architecture Analysis Features**

- **Design Pattern Analysis**: Smart/dumb components, container/presentational patterns
- **Angular Style Guide Compliance**: Naming conventions, file organization, component structure
- **Project-Specific Patterns**: Existing project patterns, conventions, and standards
- **Linting and Rules Compliance**: ESLint rules, project-specific linting standards
- **Architecture Best Practices**: SOLID principles, separation of concerns, maintainability
- **Component Relationships**: Parent-child communication, service integration, dependency management
- **Prioritized Recommendations**: Critical, high, medium, and low priority improvements
- **Angular-Specific**: Framework-specific architecture patterns and best practices

**Remember**: This command provides deep component architecture analysis to identify and prioritize Angular-specific architectural improvements, ensuring compliance with Angular Style Guide, project patterns, and architectural best practices.
