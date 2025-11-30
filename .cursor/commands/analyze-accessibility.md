# Accessibility Analysis Protocol

## Overview
Execute comprehensive accessibility analysis of Angular applications, focusing on WCAG compliance, semantic HTML, keyboard navigation, and screen reader compatibility.

## Mission Briefing: Accessibility Analysis Protocol

**ANALYSIS COMMAND DETECTION:** Before proceeding with analysis, check if the provided text contains any `/analyze*` command (e.g., `/analyze-code`, `/analyze-project`, `/analyze-bug-*`, etc.). If found, skip this command's analysis and execute the detected command instead to avoid duplicate analysis.

You will now execute comprehensive accessibility analysis using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This analysis follows the Phase 0 reconnaissance principles with specialized focus on accessibility compliance and inclusive design patterns.

---

## **Phase 0: Accessibility Reconnaissance & Mental Modeling (Read-Only)**

**Directive:** Perform non-destructive analysis of accessibility characteristics to build a complete understanding of current accessibility state.

**Accessibility Analysis Scope:**
- **WCAG Compliance**: WCAG 2.1 AA compliance analysis
- **Semantic HTML**: Proper use of semantic elements and ARIA attributes
- **Keyboard Navigation**: Full keyboard accessibility testing
- **Screen Reader Compatibility**: Screen reader support analysis
- **Color and Contrast**: Color contrast ratio validation
- **Form Accessibility**: Form labels, validation, and error handling
- **Angular-Specific**: Component accessibility patterns and best practices

**Constraints:**
- **No mutations are permitted during this phase**
- **Focus solely on understanding current accessibility characteristics**
- **Identify accessibility issues without implementing fixes**

---

## **Phase 1: WCAG Compliance Analysis**

**Directive:** Analyze WCAG 2.1 AA compliance across all accessibility principles.

**WCAG Analysis Areas:**

1. **Perceivable Analysis:**
   - Text alternatives for images and media
   - Captions and transcripts for audio/video
   - Adaptable content presentation
   - Distinguishable content (color, contrast, audio)

2. **Operable Analysis:**
   - Keyboard accessibility and navigation
   - Time limits and user control
   - Seizure and physical reaction prevention
   - Navigable content structure

3. **Understandable Analysis:**
   - Readable text and language identification
   - Predictable functionality and navigation
   - Input assistance and error prevention

4. **Robust Analysis:**
   - Compatible markup and assistive technologies
   - Future-proof accessibility implementation

**Analysis Commands:**
```bash
# Automated accessibility testing
npx pa11y http://localhost:4200
npx axe-core http://localhost:4200

# Lighthouse accessibility audit
npx lighthouse http://localhost:4200 --only-categories=accessibility

# WAVE accessibility evaluation
# Use WAVE browser extension or web service
```

---

## **Phase 2: Semantic HTML Analysis**

**Directive:** Analyze semantic HTML usage and ARIA attribute implementation.

**Semantic HTML Analysis Areas:**

1. **Element Semantics:**
   - Proper use of heading hierarchy (h1-h6)
   - Semantic sectioning elements (main, nav, article, section)
   - List structure and organization
   - Table structure and headers

2. **ARIA Implementation:**
   - ARIA labels and descriptions
   - ARIA roles and properties
   - ARIA live regions for dynamic content
   - ARIA states and properties

3. **Form Semantics:**
   - Label associations with form controls
   - Fieldset and legend usage
   - Required field indicators
   - Error message associations

4. **Interactive Elements:**
   - Button vs link usage
   - Focus management
   - Keyboard event handling
   - Screen reader announcements

**Analysis Commands:**
```bash
# HTML validation
npx html-validate src/**/*.html

# ARIA analysis
# Use browser DevTools Accessibility tab
# Check ARIA implementation

# Semantic analysis
# Use tools like axe-core for semantic issues
```

---

## **Phase 3: Keyboard Navigation Analysis**

**Directive:** Analyze keyboard navigation and focus management.

**Keyboard Analysis Areas:**

1. **Focus Management:**
   - Focus order and tab sequence
   - Focus indicators and visibility
   - Focus trapping in modals
   - Skip links and navigation shortcuts

2. **Keyboard Interactions:**
   - Keyboard event handling
   - Keyboard shortcuts and accelerators
   - Arrow key navigation
   - Enter and Space key functionality

3. **Interactive Elements:**
   - Button and link keyboard support
   - Form control keyboard navigation
   - Custom component keyboard handling
   - Menu and dropdown keyboard support

4. **Navigation Patterns:**
   - Breadcrumb navigation
   - Pagination keyboard support
   - Tab panel keyboard navigation
   - Accordion keyboard interaction

**Analysis Commands:**
```bash
# Keyboard testing
# Navigate entire application using only keyboard
# Test Tab, Shift+Tab, Enter, Space, Arrow keys

# Focus analysis
# Use browser DevTools to inspect focus states
# Check focus order and indicators
```

---

## **Phase 4: Screen Reader Compatibility Analysis**

**Directive:** Analyze screen reader compatibility and announcements.

**Screen Reader Analysis Areas:**

1. **Content Announcements:**
   - Page title and heading announcements
   - Link and button descriptions
   - Form field labels and instructions
   - Error message announcements

2. **Dynamic Content:**
   - Live region updates
   - Status message announcements
   - Progress indicator updates
   - Notification and alert announcements

3. **Navigation Support:**
   - Landmark navigation
   - Heading navigation
   - Link and button navigation
   - Form field navigation

4. **Content Structure:**
   - Heading hierarchy
   - List structure and organization
   - Table structure and headers
   - Document outline

**Analysis Commands:**
```bash
# Screen reader testing
# Test with NVDA (Windows), JAWS (Windows), or VoiceOver (Mac)
# Verify content announcements and navigation

# ARIA live regions
# Test dynamic content announcements
# Verify status updates and notifications
```

---

## **Phase 5: Color and Contrast Analysis**

**Directive:** Analyze color usage and contrast ratios.

**Color Analysis Areas:**

1. **Contrast Ratios:**
   - Normal text contrast (4.5:1 minimum)
   - Large text contrast (3:1 minimum)
   - UI component contrast
   - Focus indicator contrast

2. **Color Independence:**
   - Information not conveyed by color alone
   - Alternative indicators for color-coded information
   - Status indicators beyond color
   - Error and success indicators

3. **Color Accessibility:**
   - Colorblind-friendly palettes
   - High contrast mode support
   - Dark mode accessibility
   - Custom theme accessibility

4. **Visual Indicators:**
   - Focus indicators
   - Hover states
   - Active states
   - Disabled states

**Analysis Commands:**
```bash
# Color contrast testing
# Use WebAIM Contrast Checker
# Test all color combinations

# Colorblind simulation
# Use browser extensions or tools
# Test color accessibility

# High contrast testing
# Test with Windows High Contrast mode
# Verify all content is visible
```

---

## **Phase 6: Form Accessibility Analysis**

**Directive:** Analyze form accessibility and validation patterns.

**Form Analysis Areas:**

1. **Label Associations:**
   - Proper label associations
   - Implicit vs explicit labels
   - Grouped form controls
   - Required field indicators

2. **Error Handling:**
   - Error message associations
   - Real-time validation feedback
   - Error prevention strategies
   - Help text and instructions

3. **Input Types:**
   - Appropriate input types
   - Input constraints and validation
   - Autocomplete attributes
   - Input mode specifications

4. **Form Navigation:**
   - Tab order and navigation
   - Keyboard interaction support
   - Form submission handling
   - Reset and clear functionality

**Analysis Commands:**
```bash
# Form validation testing
# Test all form fields and validation
# Verify error message associations

# Input type testing
# Test different input types and constraints
# Verify mobile keyboard support
```

---

## **Phase 7: Angular-Specific Accessibility Analysis**

**Directive:** Analyze Angular-specific accessibility patterns and implementations using Angular accessibility best practices.

**Angular Analysis Areas:**

1. **Component Accessibility:**
   - Component host bindings
   - ARIA attribute management
   - Focus management in components
   - Keyboard event handling

2. **Routing Accessibility:**
   - Page title updates
   - Focus management on route changes
   - Breadcrumb navigation
   - Skip link implementation

3. **Form Accessibility:**
   - Reactive form accessibility
   - Template-driven form accessibility
   - Custom form control accessibility
   - Validation error handling

4. **Dynamic Content:**
   - Lazy-loaded content accessibility
   - Dynamic list accessibility
   - Modal and dialog accessibility
   - Toast notification accessibility

5. **Angular Accessibility Best Practices:**
   - Angular CDK accessibility features
   - Angular Material accessibility
   - Angular Router accessibility
   - Angular Forms accessibility
   - Angular Animations accessibility

**Angular Accessibility Best Practices Analysis:**
```typescript
// Angular CDK Accessibility Analysis
import { A11yModule } from '@angular/cdk/a11y';

@Component({
  selector: 'app-accessible-component',
  template: `
    <div cdkTrapFocus>
      <button cdkFocusInitial>First Focusable</button>
      <button>Second Focusable</button>
    </div>
    <div cdkMonitorSubtreeFocus>
      <input aria-label="Search input">
    </div>
  `
})
export class AccessibleComponent {
  // ✅ Good: Using Angular CDK accessibility features
}

// Angular Material Accessibility Analysis
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-material-accessible',
  template: `
    <button mat-raised-button 
            [attr.aria-label]="buttonLabel"
            [attr.aria-describedby]="buttonDescription">
      {{ buttonText }}
    </button>
    
    <mat-dialog-content>
      <h2 mat-dialog-title>Accessible Dialog</h2>
      <p mat-dialog-content>Dialog content with proper structure</p>
    </mat-dialog-content>
  `
})
export class MaterialAccessibleComponent {
  buttonText = 'Submit';
  buttonLabel = 'Submit form data';
  buttonDescription = 'submit-help';
  
  // ✅ Good: Using Angular Material with proper accessibility attributes
}

// Angular Router Accessibility Analysis
@Component({
  selector: 'app-accessible-routing',
  template: `
    <nav role="navigation" aria-label="Main navigation">
      <a routerLink="/home" 
         routerLinkActive="active"
         [attr.aria-current]="isActive('/home') ? 'page' : null">
        Home
      </a>
      <a routerLink="/about" 
         routerLinkActive="active"
         [attr.aria-current]="isActive('/about') ? 'page' : null">
        About
      </a>
    </nav>
  `
})
export class AccessibleRoutingComponent {
  isActive(route: string): boolean {
    return this.router.url === route;
  }
  
  // ✅ Good: Router accessibility with proper ARIA attributes
}

// Angular Forms Accessibility Analysis
@Component({
  selector: 'app-accessible-forms',
  template: `
    <form [formGroup]="form" (ngSubmit)="onSubmit()">
      <mat-form-field>
        <mat-label>Email Address</mat-label>
        <input matInput 
               type="email"
               formControlName="email"
               [attr.aria-required]="true"
               [attr.aria-invalid]="form.get('email')?.invalid"
               [attr.aria-describedby]="getAriaDescribedBy('email')">
        <mat-error *ngIf="form.get('email')?.invalid" 
                   [id]="'email-error'"
                   role="alert">
          {{ getErrorMessage('email') }}
        </mat-error>
      </mat-form-field>
    </form>
  `
})
export class AccessibleFormsComponent {
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  });
  
  getAriaDescribedBy(fieldName: string): string | null {
    const control = this.form.get(fieldName);
    if (control?.invalid && control?.touched) {
      return `${fieldName}-error`;
    }
    return null;
  }
  
  getErrorMessage(fieldName: string): string {
    const control = this.form.get(fieldName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('email')) {
      return 'Please enter a valid email address';
    }
    return '';
  }
  
  // ✅ Good: Angular Forms with comprehensive accessibility
}
```

**Analysis Commands:**
```bash
# Angular component analysis
# Check component accessibility patterns
# Verify ARIA implementation

# Route accessibility testing
# Test navigation and focus management
# Verify page title updates

# Angular CDK accessibility testing
# Test CDK accessibility features
# Verify focus management

# Angular Material accessibility testing
# Test Material component accessibility
# Verify ARIA implementation
```

---

## **Phase 8: Accessibility Compliance Report**

**Directive:** Synthesize analysis into comprehensive accessibility compliance report.

**Report Structure:**

1. **Compliance Summary:**
   - Overall accessibility score
   - WCAG 2.1 AA compliance status
   - Critical issues requiring immediate attention
   - Compliance gaps and recommendations

2. **Issue Categorization:**
   - **Critical Issues**: Blocking accessibility issues
   - **High Priority**: Significant accessibility barriers
   - **Medium Priority**: Moderate accessibility issues
   - **Low Priority**: Minor accessibility improvements

3. **Implementation Roadmap:**
   - Immediate fixes for critical issues
   - Short-term improvements for high priority issues
   - Medium-term enhancements for medium priority issues
   - Long-term accessibility strategy

4. **Testing Recommendations:**
   - Automated testing implementation
   - Manual testing procedures
   - User testing with assistive technologies
   - Continuous accessibility monitoring

**Output Requirements:**
- **Accessibility Score**: Current accessibility rating
- **Critical Issues**: Issues requiring immediate attention
- **Compliance Gaps**: WCAG 2.1 AA compliance gaps
- **Implementation Guide**: Step-by-step accessibility improvements
- **Testing Strategy**: Comprehensive accessibility testing plan

---

## **Usage Examples**

- `/analyze-accessibility` - Analyze entire application accessibility
- `/analyze-accessibility @src/app/` - Analyze specific application directory
- `/analyze-accessibility @src/components/` - Analyze component accessibility
- `/analyze-accessibility @src/components/user-profile.component.ts` - Analyze specific component
- `/analyze-accessibility --wcag-only` - Focus on WCAG compliance only
- `/analyze-accessibility --keyboard-only` - Focus on keyboard navigation only
- `/analyze-accessibility --angular-only` - Focus on Angular-specific accessibility patterns

## **Accessibility Analysis Features**

- **WCAG Compliance**: Comprehensive WCAG 2.1 AA compliance analysis
- **Semantic HTML**: Semantic element and ARIA attribute analysis
- **Keyboard Navigation**: Full keyboard accessibility testing
- **Screen Reader**: Screen reader compatibility analysis
- **Color Contrast**: Color and contrast ratio validation
- **Form Accessibility**: Form accessibility and validation analysis
- **Angular-Specific**: Framework-specific accessibility patterns and best practices
- **Angular CDK**: Angular CDK accessibility features analysis
- **Angular Material**: Angular Material component accessibility analysis
- **Angular Router**: Angular Router accessibility patterns
- **Angular Forms**: Angular Forms accessibility implementation
- **Component Analysis**: Deep component-level accessibility analysis
- **Actionable Recommendations**: Prioritized accessibility improvement roadmap

**Remember**: This command provides comprehensive accessibility analysis to ensure inclusive design and equal access for all users, regardless of their abilities or the technology they use, with specialized focus on Angular accessibility best practices.
