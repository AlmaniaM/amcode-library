# Fix Lint Errors Command

## Overview

Automatically fix ESLint errors classified as ERROR level only. This command understands the project's linting rules and applies safe, automated fixes without modifying code logic or introducing breaking changes.

## Mission Briefing: Lint Error Fixing Protocol

**LINT ERROR FIXING COMMAND:** Execute systematic fixing of ESLint errors using the **AUTONOMOUS PRINCIPAL ENGINEER - OPERATIONAL DOCTRINE.** This command focuses exclusively on ERROR level issues, leaving warnings and style issues untouched.

---

## **Phase 0: Lint Error Analysis & Classification (Read-Only)**

**Directive:** Analyze current linting errors and classify them by severity and fixability.

**Error Classification Scope:**

- **ERROR Level Only**: Focus exclusively on errors that prevent compilation or cause runtime issues
- **Safe Fixes Only**: Only apply fixes that don't change code logic or behavior
- **Automated Fixes**: Use ESLint's `--fix` capability for safe transformations
- **Manual Review Required**: Identify errors that require human intervention

**Constraints:**

- **No mutations are permitted during this phase**
- **Only analyze ERROR level issues**
- **Identify fixable vs non-fixable errors**

---

## **Phase 1: Lint Configuration Analysis**

**Directive:** Understand the project's ESLint configuration and available fix rules.

**Configuration Analysis:**

1. **ESLint Configuration Review**:
   - Parse `.eslintrc.json` and related configs
   - Identify rules that support `--fix` functionality
   - Map error types to fix strategies
   - Understand Angular-specific rules and fixes

2. **Fixable Rule Identification**:
   - **Import/Export Rules**: Fix import ordering, missing imports, unused imports
   - **Formatting Rules**: Fix spacing, semicolons, quotes (when using Prettier integration)
   - **TypeScript Rules**: Fix type annotations, interface definitions
   - **Angular Rules**: Fix template syntax, component patterns
   - **Code Style Rules**: Fix naming conventions, code structure

3. **Non-Fixable Error Identification**:
   - **Logic Errors**: Code that requires understanding of business logic
   - **Architecture Violations**: Module boundary violations, dependency issues
   - **Custom Rule Violations**: Project-specific rules that need manual intervention
   - **Complex Type Errors**: TypeScript errors requiring design decisions

---

## **Phase 2: Error Detection & Prioritization**

**Directive:** Run ESLint and identify ERROR level issues that can be safely fixed.

**Error Detection Process:**

1. **ESLint Execution**:
   ```bash
   # Run ESLint with error-only output
   npx eslint . --format=json --quiet
   
   # Run ESLint on specific files if targeting
   npx eslint [target-files] --format=json --quiet
   ```

2. **Error Filtering**:
   - Filter results to ERROR severity only
   - Exclude warnings and info-level issues
   - Group errors by file and rule
   - Identify patterns in error types

3. **Fixability Assessment**:
   - Check if error rule supports `--fix`
   - Assess risk of automated fix
   - Identify errors requiring manual intervention
   - Prioritize fixes by impact and safety

---

## **Phase 3: Safe Automated Fixing**

**Directive:** Apply automated fixes for ERROR level issues that can be safely resolved.

**Automated Fix Process:**

1. **ESLint Auto-Fix Execution**:
   ```bash
   # Apply fixes to all files
   npx eslint . --fix --quiet
   
   # Apply fixes to specific files
   npx eslint [target-files] --fix --quiet
   ```

2. **Fix Verification**:
   - Re-run ESLint to verify fixes
   - Check for new errors introduced
   - Validate TypeScript compilation
   - Ensure no breaking changes

3. **Incremental Fixing**:
   - Fix one file at a time if needed
   - Verify each fix before proceeding
   - Rollback if issues are introduced
   - Document any manual fixes required

---

## **Phase 4: Manual Fix Identification**

**Directive:** Identify ERROR level issues that require manual intervention and provide guidance.

**Manual Fix Categories:**

1. **Import/Export Issues**:
   - Missing module declarations
   - Circular dependency issues
   - Incorrect import paths
   - Missing type definitions

2. **TypeScript Errors**:
   - Type mismatches requiring design decisions
   - Missing interface implementations
   - Generic type constraints
   - Complex union type issues

3. **Angular-Specific Errors**:
   - Component lifecycle issues
   - Template syntax errors
   - Service injection problems
   - Module configuration issues

4. **Architecture Violations**:
   - Module boundary violations
   - Dependency constraint violations
   - Circular dependency issues
   - Nx workspace rule violations

---

## **Phase 5: Fix Verification & Validation**

**Directive:** Verify all fixes are correct and don't introduce new issues.

**Verification Process:**

1. **Lint Re-check**:
   ```bash
   # Verify no ERROR level issues remain
   npx eslint . --format=json --quiet
   ```

2. **TypeScript Compilation**:
   ```bash
   # Ensure TypeScript compilation succeeds
   npx tsc --noEmit
   ```

3. **Build Verification**:
   ```bash
   # Verify build process works
   npm run build
   ```

4. **Test Execution**:
   ```bash
   # Run tests to ensure no regressions
   npm test
   ```

---

## **Phase 6: Fix Report & Documentation**

**Directive:** Document all fixes applied and remaining issues requiring manual attention.

**Report Structure:**

### **Fixed Errors Summary**
- **Total Errors Fixed**: Count of successfully fixed errors
- **Files Modified**: List of files with automated fixes
- **Fix Types Applied**: Categories of fixes applied
- **Time Saved**: Estimated time saved by automated fixes

### **Remaining Manual Fixes Required**
- **Error Count**: Number of errors requiring manual intervention
- **Error Categories**: Types of errors that couldn't be auto-fixed
- **File Locations**: Specific files and line numbers
- **Fix Guidance**: Recommended approaches for manual fixes

### **Verification Results**
- **Lint Status**: Current lint status after fixes
- **Build Status**: Build process verification results
- **Test Status**: Test execution results
- **TypeScript Status**: TypeScript compilation results

---

## **Usage Examples**

- `/fix-lint-errors` - Fix all ERROR level lint issues in the project
- `/fix-lint-errors @src/components/` - Fix ERROR level issues in specific directory
- `/fix-lint-errors @src/app/app.component.ts` - Fix ERROR level issues in specific file
- `/fix-lint-errors --dry-run` - Analyze errors without applying fixes

## **Supported Fix Types**

### **Automatically Fixable**
- Import/export ordering and organization
- Missing semicolons and basic formatting
- Unused imports and variables
- Basic TypeScript type annotations
- Angular template syntax corrections
- Code style and naming conventions

### **Requires Manual Intervention**
- Business logic errors
- Complex type definitions
- Architecture violations
- Custom rule violations
- Circular dependencies
- Missing dependencies

## **Safety Features**

- **Read-Only Analysis**: Phase 0 is completely read-only
- **Incremental Fixing**: Apply fixes one file at a time
- **Rollback Capability**: Ability to revert problematic fixes
- **Verification Steps**: Multiple verification layers
- **Error Prevention**: Stop if new errors are introduced

## **Integration with Project**

- **ESLint Configuration**: Uses project's `.eslintrc.json`
- **TypeScript Integration**: Respects `tsconfig.json` settings
- **Nx Workspace**: Works with Nx monorepo structure
- **Angular Rules**: Supports Angular-specific ESLint rules
- **Prettier Integration**: Works with Prettier formatting rules

**Remember**: This command focuses exclusively on ERROR level issues that can be safely fixed automatically. It prioritizes code safety and maintains existing functionality while resolving linting errors.
