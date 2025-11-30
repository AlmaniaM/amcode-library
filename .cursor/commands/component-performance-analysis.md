# Component Performance Analysis Protocol

## Overview
Execute deep performance analysis of Angular components, focusing on change detection optimization, zone pollution prevention, slow computations, signals implementation, and subtree skipping strategies.

## Mission Briefing: Component Performance Analysis Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute deep component performance analysis using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows the Phase 0 reconnaissance principles with specialized focus on Angular component performance optimization.

---

## **Phase 0: Component Performance Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of Angular component files (.ts and .html) to build a complete understanding of current performance characteristics.

**Component Performance Analysis Scope:**
- **Change Detection Analysis**: OnPush strategy, change detection frequency, trackBy functions
- **Zone Pollution Prevention**: Zone.js optimization, third-party library integration
- **Slow Computations**: Heavy computations in templates, expensive operations
- **Signals Implementation**: Angular signals usage and optimization opportunities
- **Subtree Skipping**: Component tree optimization and change detection skipping
- **Angular Style Guide Compliance**: Performance best practices adherence

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus solely on understanding current component performance state**
- **Analyze both .ts and .html files comprehensively**

---

## **Phase 1: Change Detection Analysis**

**Directive:** Analyze change detection strategy and optimization opportunities.

**Change Detection Analysis Areas:**

1. **Change Detection Strategy:**
   - Current change detection strategy (Default vs OnPush)
   - ChangeDetectionStrategy implementation
   - ChangeDetectorRef usage patterns
   - Manual change detection triggers

2. **Template Performance:**
   - Function calls in templates
   - Complex expressions in templates
   - Pipe usage and optimization
   - Structural directive performance

3. **TrackBy Functions:**
   - *ngFor trackBy implementation
   - TrackBy function optimization
   - List rendering performance
   - DOM manipulation efficiency

4. **Async Operations:**
   - Observable subscription patterns
   - Async pipe usage
   - Promise handling
   - Subscription management

**Change Detection Analysis Patterns:**
```typescript
// OnPush Strategy Analysis
@Component({
  selector: 'app-performance-component',
  changeDetection: ChangeDetectionStrategy.OnPush, // ✅ Good
  template: `
    <div *ngFor="let item of items; trackBy: trackByFn"> <!-- ✅ Good -->
      {{ item.name }}
    </div>
    <div>{{ getExpensiveValue() }}</div> <!-- ❌ Bad: Function in template -->
  `
})
export class PerformanceComponent {
  items: Item[] = [];
  
  // ✅ Good: TrackBy function
  trackByFn(index: number, item: Item): any {
    return item.id;
  }
  
  // ❌ Bad: Expensive computation in template
  getExpensiveValue(): string {
    return this.items.reduce((acc, item) => acc + item.value, 0).toString();
  }
}
```

**Change Detection Analysis Commands:**
```bash
# Change detection profiling
# Use Angular DevTools browser extension
# Profile change detection cycles

# Template analysis
# Check for function calls in templates
# Analyze expression complexity
```

---

## **Phase 2: Zone Pollution Prevention Analysis**

**Directive:** Analyze zone pollution and third-party library integration.

**Zone Pollution Analysis Areas:**

1. **Zone.js Integration:**
   - Third-party library zone integration
   - Event listener zone pollution
   - Timer and interval zone pollution
   - Promise and async operation pollution

2. **Event Handling:**
   - Event listener registration
   - Event handler performance
   - Event delegation patterns
   - Custom event handling

3. **Third-Party Libraries:**
   - Library zone compatibility
   - Zone-free library usage
   - Library performance impact
   - Integration optimization

4. **Async Operations:**
   - Promise handling patterns
   - Observable zone integration
   - Async/await usage
   - Error handling in async operations

**Zone Pollution Analysis Patterns:**
```typescript
// Zone Pollution Prevention
@Component({
  selector: 'app-zone-optimized',
  template: `...`
})
export class ZoneOptimizedComponent {
  constructor(private ngZone: NgZone) {}
  
  // ✅ Good: Zone-free operation
  performHeavyComputation(): void {
    this.ngZone.runOutsideAngular(() => {
      // Heavy computation that doesn't need change detection
      const result = this.calculateExpensiveValue();
      
      // Run back in zone when done
      this.ngZone.run(() => {
        this.updateUI(result);
      });
    });
  }
  
  // ❌ Bad: Zone pollution from third-party library
  setupThirdPartyLibrary(): void {
    // This might trigger change detection unnecessarily
    this.thirdPartyLib.on('update', () => {
      this.data = this.thirdPartyLib.getData();
    });
  }
  
  // ✅ Good: Zone-free third-party integration
  setupOptimizedThirdPartyLibrary(): void {
    this.ngZone.runOutsideAngular(() => {
      this.thirdPartyLib.on('update', () => {
        this.ngZone.run(() => {
          this.data = this.thirdPartyLib.getData();
        });
      });
    });
  }
}
```

---

## **Phase 3: Slow Computations Analysis**

**Directive:** Analyze slow computations and expensive operations.

**Slow Computations Analysis Areas:**

1. **Template Computations:**
   - Function calls in templates
   - Complex expressions in templates
   - Heavy computations in getters
   - Repeated calculations

2. **Component Computations:**
   - Heavy operations in component methods
   - Expensive data transformations
   - Complex filtering and sorting
   - Memory-intensive operations

3. **Service Computations:**
   - Heavy service operations
   - Expensive API calls
   - Complex data processing
   - Caching opportunities

4. **Pipe Computations:**
   - Custom pipe performance
   - Pipe caching opportunities
   - Expensive pipe transformations
   - Pipe optimization patterns

**Slow Computations Analysis Patterns:**
```typescript
// Slow Computations Analysis
@Component({
  selector: 'app-slow-computations',
  template: `
    <!-- ❌ Bad: Function calls in template -->
    <div>{{ getExpensiveValue() }}</div>
    <div>{{ calculateComplexData() }}</div>
    
    <!-- ✅ Good: Computed properties -->
    <div>{{ expensiveValue }}</div>
    <div>{{ complexData }}</div>
    
    <!-- ✅ Good: Async pipe for observables -->
    <div>{{ data$ | async }}</div>
  `
})
export class SlowComputationsComponent {
  data: any[] = [];
  
  // ❌ Bad: Expensive computation in getter
  get expensiveValue(): string {
    return this.data.reduce((acc, item) => acc + item.value, 0).toString();
  }
  
  // ❌ Bad: Function called in template
  getExpensiveValue(): string {
    return this.data.reduce((acc, item) => acc + item.value, 0).toString();
  }
  
  // ✅ Good: Cached computation
  private _expensiveValue: string | null = null;
  get expensiveValue(): string {
    if (this._expensiveValue === null) {
      this._expensiveValue = this.data.reduce((acc, item) => acc + item.value, 0).toString();
    }
    return this._expensiveValue;
  }
  
  // ✅ Good: Computed property with change detection
  get complexData(): any {
    return this.data.filter(item => item.active).map(item => ({
      ...item,
      processed: true
    }));
  }
}
```

---

## **Phase 4: Signals Implementation Analysis**

**Directive:** Analyze Angular signals usage and optimization opportunities.

**Signals Analysis Areas:**

1. **Signal Usage:**
   - Signal implementation patterns
   - Signal vs Observable usage
   - Signal performance benefits
   - Signal change detection optimization

2. **Computed Signals:**
   - Computed signal implementation
   - Computed signal dependencies
   - Computed signal performance
   - Computed signal optimization

3. **Effect Usage:**
   - Effect implementation patterns
   - Effect cleanup and management
   - Effect performance impact
   - Effect optimization opportunities

4. **Signal Integration:**
   - Signal integration with components
   - Signal integration with services
   - Signal integration with forms
   - Signal integration with routing

**Signals Analysis Patterns:**
```typescript
// Signals Implementation Analysis
@Component({
  selector: 'app-signals-component',
  template: `
    <div>{{ count() }}</div>
    <div>{{ doubleCount() }}</div>
    <button (click)="increment()">Increment</button>
  `
})
export class SignalsComponent {
  // ✅ Good: Signal implementation
  count = signal(0);
  
  // ✅ Good: Computed signal
  doubleCount = computed(() => this.count() * 2);
  
  // ✅ Good: Signal-based method
  increment(): void {
    this.count.update(value => value + 1);
  }
  
  // ✅ Good: Effect for side effects
  constructor() {
    effect(() => {
      console.log('Count changed:', this.count());
    });
  }
}

// Signal vs Observable Analysis
@Injectable()
export class SignalOptimizedService {
  // ✅ Good: Signal for reactive state
  private _data = signal<any[]>([]);
  data = this._data.asReadonly();
  
  // ✅ Good: Computed signal for derived state
  filteredData = computed(() => 
    this._data().filter(item => item.active)
  );
  
  // ✅ Good: Signal-based updates
  updateData(newData: any[]): void {
    this._data.set(newData);
  }
  
  // ❌ Bad: Observable for simple state (if signals are available)
  data$ = new BehaviorSubject<any[]>([]);
}
```

---

## **Phase 5: Subtree Skipping Analysis**

**Directive:** Analyze component tree optimization and change detection skipping.

**Subtree Skipping Analysis Areas:**

1. **Component Tree Structure:**
   - Component hierarchy analysis
   - Component dependency analysis
   - Component isolation opportunities
   - Component tree optimization

2. **Change Detection Skipping:**
   - OnPush strategy implementation
   - Change detection skipping patterns
   - Manual change detection triggers
   - Change detection optimization

3. **Component Isolation:**
   - Component boundary optimization
   - Component communication patterns
   - Component state management
   - Component lifecycle optimization

4. **Performance Optimization:**
   - Component rendering optimization
   - Component update optimization
   - Component memory optimization
   - Component performance monitoring

**Subtree Skipping Analysis Patterns:**
```typescript
// Subtree Skipping Analysis
@Component({
  selector: 'app-parent',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <app-child [data]="data" (update)="onUpdate($event)"></app-child>
    <app-isolated-child></app-isolated-child>
  `
})
export class ParentComponent {
  data = signal<any[]>([]);
  
  // ✅ Good: Signal-based updates
  onUpdate(newData: any[]): void {
    this.data.set(newData);
  }
}

@Component({
  selector: 'app-child',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div *ngFor="let item of data()">{{ item.name }}</div>
  `
})
export class ChildComponent {
  @Input() data = signal<any[]>([]);
  @Output() update = new EventEmitter<any[]>();
  
  // ✅ Good: OnPush with signal input
  // Change detection only runs when signal changes
}

@Component({
  selector: 'app-isolated-child',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div>Isolated component</div>
  `
})
export class IsolatedChildComponent {
  // ✅ Good: Isolated component
  // No external dependencies, minimal change detection
}
```

---

## **Phase 6: Angular Style Guide Compliance Analysis**

**Directive:** Analyze compliance with Angular Style Guide performance best practices.

**Style Guide Analysis Areas:**

1. **Component Design:**
   - Component responsibility analysis
   - Component size and complexity
   - Component naming conventions
   - Component organization

2. **Template Design:**
   - Template structure and organization
   - Template performance patterns
   - Template accessibility
   - Template maintainability

3. **Service Design:**
   - Service responsibility analysis
   - Service performance patterns
   - Service dependency management
   - Service testing patterns

4. **Performance Patterns:**
   - Performance best practices adherence
   - Anti-pattern identification
   - Optimization opportunities
   - Performance monitoring

**Style Guide Analysis Patterns:**
```typescript
// Angular Style Guide Compliance Analysis
@Component({
  selector: 'app-style-guide-compliant',
  changeDetection: ChangeDetectionStrategy.OnPush, // ✅ Style Guide: OnPush
  template: `
    <div class="component-container">
      <h2>{{ title }}</h2>
      <div *ngFor="let item of items(); trackBy: trackByFn" class="item">
        {{ item.name }}
      </div>
    </div>
  `
})
export class StyleGuideCompliantComponent {
  // ✅ Style Guide: Input properties
  @Input() title = '';
  @Input() items = signal<any[]>([]);
  
  // ✅ Style Guide: Output properties
  @Output() itemSelected = new EventEmitter<any>();
  
  // ✅ Style Guide: TrackBy function
  trackByFn(index: number, item: any): any {
    return item.id;
  }
  
  // ✅ Style Guide: Lifecycle hooks
  ngOnInit(): void {
    // Initialization logic
  }
  
  ngOnDestroy(): void {
    // Cleanup logic
  }
}
```

---

## **Phase 7: Performance Recommendations and Prioritization**

**Directive:** Synthesize analysis into prioritized performance recommendations.

**Recommendation Categories:**

1. **Critical Issues (High Priority):**
   - Function calls in templates
   - Missing OnPush strategy
   - Zone pollution from third-party libraries
   - Memory leaks and subscription issues

2. **High Priority Improvements:**
   - Missing trackBy functions
   - Expensive computations in templates
   - Inefficient change detection patterns
   - Missing signal implementation opportunities

3. **Medium Priority Optimizations:**
   - Component tree optimization
   - Service performance improvements
   - Template structure optimization
   - Caching implementation opportunities

4. **Low Priority Enhancements:**
   - Code organization improvements
   - Documentation enhancements
   - Monitoring implementation
   - Future optimization opportunities

**Performance Recommendations Output:**
```markdown
## Component Performance Analysis Report

### Critical Issues (Fix Immediately)
1. **Function Calls in Template** - HIGH IMPACT
   - Location: Template line 15
   - Issue: `{{ getExpensiveValue() }}` called on every change detection
   - Solution: Move to computed property or use OnPush strategy
   - Priority: CRITICAL

2. **Missing OnPush Strategy** - HIGH IMPACT
   - Location: Component decorator
   - Issue: Using default change detection strategy
   - Solution: Implement ChangeDetectionStrategy.OnPush
   - Priority: CRITICAL

### High Priority Improvements
1. **Missing TrackBy Function** - MEDIUM IMPACT
   - Location: *ngFor directive
   - Issue: No trackBy function for list rendering
   - Solution: Implement trackBy function
   - Priority: HIGH

2. **Zone Pollution** - MEDIUM IMPACT
   - Location: Third-party library integration
   - Issue: Library events triggering unnecessary change detection
   - Solution: Use ngZone.runOutsideAngular()
   - Priority: HIGH

### Medium Priority Optimizations
1. **Signal Implementation Opportunity** - LOW IMPACT
   - Location: Component state management
   - Issue: Using BehaviorSubject instead of signals
   - Solution: Migrate to Angular signals
   - Priority: MEDIUM

2. **Component Tree Optimization** - LOW IMPACT
   - Location: Component hierarchy
   - Issue: Unnecessary component nesting
   - Solution: Flatten component structure
   - Priority: MEDIUM
```

---

## **Usage Examples**

- `/component-performance-analysis @src/components/user-profile.component.ts` - Analyze specific component
- `/component-performance-analysis @src/components/` - Analyze component directory
- `/component-performance-analysis --change-detection-only` - Focus on change detection analysis
- `/component-performance-analysis --signals-only` - Focus on signals implementation
- `/component-performance-analysis --zone-pollution-only` - Focus on zone pollution analysis

## **Component Performance Analysis Features**

- **Change Detection Analysis**: OnPush strategy, trackBy functions, template optimization
- **Zone Pollution Prevention**: Third-party library integration, event handling optimization
- **Slow Computations**: Template performance, expensive operations, caching opportunities
- **Signals Implementation**: Angular signals usage, computed signals, effect optimization
- **Subtree Skipping**: Component tree optimization, change detection skipping
- **Style Guide Compliance**: Angular Style Guide performance best practices
- **Prioritized Recommendations**: Critical, high, medium, and low priority improvements
- **Angular-Specific**: Framework-specific performance patterns and optimization

**Remember**: This command provides deep component performance analysis to identify and prioritize Angular-specific performance optimizations, ensuring optimal change detection, zone pollution prevention, and modern Angular patterns.
