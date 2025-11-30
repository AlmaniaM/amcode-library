# Component Migration Command Protocol

## Overview
Systematic migration of individual components from cartalk_frontend to cartalk with 1-to-1 translation and AS-IS preservation.

## Command Usage
```
/migrate path/to/Component.js
```

## Mission Briefing: Component Migration Protocol

You will execute a systematic component migration following the **CarTalk Consolidation Migration Protocol** with focus on AS-IS preservation and minimal changes.

---

## **Phase 0: Component Analysis (Read-Only)**

**Directive:** Perform comprehensive analysis of the source component before any migration action.

### **Step 1: Source Component Analysis**
1. **Read Complete Source File**: Full content analysis of the target component
2. **Import Chain Analysis**: Map ALL imported dependencies
3. **Dependency Tree Mapping**: Recursively analyze ALL component dependencies
4. **Styling Dependencies**: Identify CSS, theme, and styling requirements
5. **Context Dependencies**: Map any context or state management requirements
6. **External Dependencies**: Identify API calls, services, and external integrations

### **Step 2: Target Location Verification**
1. **Path Structure Analysis**: Determine correct location in cartalk project
2. **Existing Component Check**: Verify if component already exists
3. **Dependency Availability**: Check if ALL dependencies exist in target project
4. **Import Path Mapping**: Map source imports to target import paths

---

## **Phase 1: Dependency Resolution**

**Directive:** Ensure ALL dependencies are available before migrating the component.

### **Step 1: Missing Dependency Identification**
1. **Component Dependencies**: List ALL missing imported components
2. **Helper Dependencies**: Identify missing utility functions and helpers
3. **Style Dependencies**: Map missing CSS files and theme requirements
4. **Context Dependencies**: Identify missing context providers

### **Step 2: Dependency Migration**
1. **Recursive Migration**: Migrate ALL missing dependencies first
2. **Helper Function Migration**: Migrate required utility functions
3. **Style Migration**: Migrate CSS files and styling dependencies
4. **Context Migration**: Migrate required context providers

---

## **Phase 2: Component Migration**

**Directive:** Migrate the component with 1-to-1 translation preserving ALL functionality.

### **Step 1: File Transfer**
1. **AS-IS Copy**: Copy exact component structure and logic
2. **Extension Conversion**: Change .js/.jsx to .ts/.tsx
3. **Preserve Comments**: Maintain ALL original comments and structure
4. **Preserve Logic**: Do NOT modify business logic or functionality

### **Step 2: TypeScript Conversion**
1. **Interface Creation**: Add TypeScript interfaces for ALL props
2. **Type Annotations**: Add type annotations for ALL variables and functions
3. **Import Type Updates**: Update imports with proper TypeScript types
4. **Generic Types**: Add generic types where appropriate
5. **JSDoc Preservation**: Maintain ALL existing JSDoc comments

### **Step 3: Import Resolution**
1. **Path Updates**: Update import paths to match cartalk project structure
2. **Alias Updates**: Update path aliases (@/, ~/, etc.) to cartalk config
3. **Dependency Imports**: Ensure ALL imports resolve correctly
4. **External Dependencies**: Verify ALL external package imports

### **Step 4: Style Integration**
1. **Theme Integration**: Update theme imports to use cartalk theme
2. **CSS Migration**: Migrate associated CSS files if needed
3. **Styling Preservation**: Maintain exact visual appearance
4. **Responsive Design**: Preserve ALL responsive design patterns

---

## **Phase 3: Integration & Verification**

**Directive:** Integrate the component and verify complete functionality.

### **Step 1: Renderer Integration**
1. **Import Addition**: Add component import to Renderer.tsx
2. **Case Addition**: Add component case to switch statement
3. **Type Integration**: Ensure proper typing in Renderer
4. **Export Verification**: Verify component exports correctly

### **Step 2: Compilation Verification**
1. **TypeScript Check**: Verify TypeScript compilation succeeds
2. **Import Resolution**: Confirm ALL imports resolve correctly
3. **Type Safety**: Ensure no TypeScript errors
4. **Build Verification**: Verify component builds without errors

### **Step 3: Functionality Verification**
1. **Component Rendering**: Verify component renders without errors
2. **Props Handling**: Test component with expected props
3. **Event Handling**: Verify ALL event handlers work correctly
4. **State Management**: Test component state management
5. **Visual Accuracy**: Confirm visual appearance matches original

---

## **Mandatory Migration Rules**

### **AS-IS Preservation**
- **FORBIDDEN**: Modifying business logic during migration
- **FORBIDDEN**: Changing component functionality or behavior
- **FORBIDDEN**: Updating styling beyond TypeScript conversion
- **FORBIDDEN**: Refactoring or optimizing during migration
- **REQUIRED**: Preserve exact functionality and appearance

### **Dependency Management**
- **REQUIRED**: Migrate ALL dependencies before component
- **REQUIRED**: Verify ALL imports resolve correctly
- **REQUIRED**: Maintain dependency chain integrity
- **FORBIDDEN**: Skipping dependency migration

### **TypeScript Conversion**
- **REQUIRED**: 100% TypeScript coverage
- **REQUIRED**: Proper interface definitions for ALL props
- **REQUIRED**: Type annotations for ALL variables and functions
- **FORBIDDEN**: Using 'any' types unless absolutely necessary

### **Quality Assurance**
- **REQUIRED**: Zero TypeScript compilation errors
- **REQUIRED**: Zero linting errors
- **REQUIRED**: Functional component rendering
- **REQUIRED**: Visual accuracy preservation

---

## **Migration Workflow Template**

### **Pre-Migration Checklist**
- [ ] Source component analysis completed
- [ ] ALL dependencies identified and mapped
- [ ] Target location determined
- [ ] Existing component conflicts resolved

### **Migration Execution**
- [ ] ALL dependencies migrated
- [ ] Component structure preserved
- [ ] TypeScript interfaces created
- [ ] Import paths updated
- [ ] Styling integrated

### **Post-Migration Verification**
- [ ] TypeScript compilation successful
- [ ] Component imports and exports correctly
- [ ] Renderer integration completed
- [ ] Functionality verification passed
- [ ] Visual accuracy confirmed

---

## **Error Recovery Protocol**

### **Migration Failures**
1. **Stop Immediately**: Do not proceed if ANY step fails
2. **Identify Root Cause**: Determine why migration failed
3. **Resolve Dependencies**: Fix missing dependencies first
4. **Retry Migration**: Re-attempt migration after fixes
5. **Verify Integrity**: Ensure no regressions introduced

### **Common Failure Patterns**
- **Missing Dependencies**: Migrate dependencies first
- **Import Path Errors**: Update path aliases and imports
- **TypeScript Errors**: Fix type definitions and interfaces
- **Styling Issues**: Ensure theme and CSS integration

---

## **Success Criteria**

### **Component Migration Success**
- **Functionality**: 100% original functionality preserved
- **Appearance**: Visual appearance matches original exactly
- **TypeScript**: 100% TypeScript coverage with strict typing
- **Integration**: Seamless integration with cartalk project
- **Performance**: No performance degradation

### **Quality Metrics**
- **Zero Errors**: No TypeScript, linting, or build errors
- **Complete Coverage**: ALL dependencies migrated
- **Exact Preservation**: AS-IS functionality maintained
- **Proper Integration**: Component works in target environment

**Remember**: This migration command ensures systematic, error-free component migration while preserving exact functionality and appearance. Every migration must maintain the integrity of the original component while achieving full TypeScript compatibility.
