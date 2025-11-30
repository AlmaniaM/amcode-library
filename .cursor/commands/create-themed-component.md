# Create Themed Component Command

## Purpose
Create themed wrapper components that use theme system for all styling values, enabling rapid theme switching. This command wraps existing components with theme-aware versions that reference the provided component design directions file for design system specifications.

## When to Use
- **Component Theming**: When creating themed versions of existing components
- **Theme System Integration**: When migrating components to use theme system exclusively
- **Design System Compliance**: When ensuring components follow design directions specifications
- **Rapid Theme Switching**: When enabling components to switch themes dynamically
- **Code Consistency**: When standardizing component styling across the application

## Command Structure

### Phase 1: Analysis & Preparation
**AUTONOMOUS PRINCIPAL ENGINEER DOCTRINE**: Begin with comprehensive understanding of the target component and design directions.

#### Component Analysis
- [ ] **REQUIRED**: Read the target component file completely
- [ ] **REQUIRED**: Identify all hardcoded values (colors, spacing, border radius, typography)
- [ ] **REQUIRED**: Map hardcoded values to theme properties
- [ ] **REQUIRED**: Identify Text/TextInput usage that should use ThemedText/ThemedTextInput
- [ ] **REQUIRED**: Document component props and their usage

#### Design Directions Analysis
- [ ] **REQUIRED**: Read the provided component design directions file completely
- [ ] **REQUIRED**: Extract design tokens (colors, spacing, border radius, typography) from design directions
- [ ] **REQUIRED**: Identify design system specifications (border radius values, spacing scale, typography scale)
- [ ] **REQUIRED**: Verify design tokens exist in theme system
- [ ] **REQUIRED**: Add missing design tokens to theme if needed

#### Theme System Verification
- [ ] **REQUIRED**: Check `recipe-ocr-mobile-app/src/constants/themes.ts` for available theme properties
- [ ] **REQUIRED**: Verify all required colors exist in theme.colors
- [ ] **REQUIRED**: Verify all required spacing exists in theme.spacing
- [ ] **REQUIRED**: Verify all required border radius exists in theme.borderRadius
- [ ] **REQUIRED**: Add missing theme properties if needed

### Phase 2: Theme Property Mapping
**Systematic Value Replacement**: Map all hardcoded values to theme properties based on design directions.

#### Color Mapping
- [ ] **REQUIRED**: Map all hex colors to `theme.colors.*` properties
- [ ] **REQUIRED**: Map opacity values to `theme.opacity.*` properties
- [ ] **REQUIRED**: Map rgba colors to theme color + opacity combination
- [ ] **REQUIRED**: Document color mapping decisions
- [ ] **REQUIRED**: Verify color contrast meets accessibility requirements

#### Spacing Mapping
- [ ] **REQUIRED**: Map all pixel spacing values to `theme.spacing.*` properties
- [ ] **REQUIRED**: Map margin/padding values to theme spacing tokens
- [ ] **REQUIRED**: Map gap values to theme spacing tokens
- [ ] **REQUIRED**: Document spacing mapping decisions
- [ ] **REQUIRED**: Verify spacing follows design system scale from design directions

#### Border Radius Mapping
- [ ] **REQUIRED**: Map all border radius values to `theme.borderRadius.*` properties
- [ ] **REQUIRED**: Reference design directions file for border radius specifications (cards, pills, inputs, etc.)
- [ ] **REQUIRED**: Map card/container radius to appropriate `theme.borderRadius.*` property per design directions
- [ ] **REQUIRED**: Map pill/chip radius to appropriate `theme.borderRadius.*` property per design directions
- [ ] **REQUIRED**: Map input radius to appropriate `theme.borderRadius.*` property per design directions
- [ ] **REQUIRED**: Document border radius mapping decisions

#### Typography Mapping
- [ ] **REQUIRED**: Replace Text components with ThemedText where applicable
- [ ] **REQUIRED**: Replace TextInput components with ThemedTextInput where applicable
- [ ] **REQUIRED**: Map fontSize/fontWeight/lineHeight to ThemedText variants
- [ ] **REQUIRED**: Remove fontFamily from StyleSheet (use ThemedText instead)
- [ ] **REQUIRED**: Document typography mapping decisions

### Phase 3: Folder Structure Reorganization
**Component Organization**: Reorganize component into folder structure with index file.

#### Folder Structure Creation
- [ ] **REQUIRED**: Create folder named after component (e.g., `SearchBar/` for `SearchBar.tsx`)
- [ ] **REQUIRED**: Move original component file into the new folder
- [ ] **REQUIRED**: Update import paths in the moved component if needed (relative imports)
- [ ] **REQUIRED**: Identify all files that import the original component
- [ ] **REQUIRED**: Document import path changes needed

#### Index File Creation
- [ ] **REQUIRED**: Create `index.ts` file in the component folder
- [ ] **REQUIRED**: Export the original component from `index.ts`
- [ ] **REQUIRED**: Export the themed component from `index.ts` (will be created in Phase 4)
- [ ] **REQUIRED**: Export component types/props if applicable
- [ ] **REQUIRED**: Ensure index file follows barrel export pattern

#### Import Path Updates
- [ ] **REQUIRED**: Update all imports of the original component to use the new folder path
- [ ] **REQUIRED**: Verify all imports resolve correctly after reorganization
- [ ] **REQUIRED**: Test that existing usage of the component still works
- [ ] **REQUIRED**: Document any breaking import path changes

### Phase 4: Themed Component Creation
**Wrapper Component Implementation**: Create theme-aware wrapper component in the organized folder structure.

#### Wrapper Component Structure
- [ ] **REQUIRED**: Create `Themed<ComponentName>.tsx` file in the component folder
- [ ] **REQUIRED**: Import original component from `./<ComponentName>` (same folder)
- [ ] **REQUIRED**: Import `useTheme` hook from ThemeContext
- [ ] **REQUIRED**: Import ThemedText/ThemedTextInput if needed
- [ ] **REQUIRED**: Preserve all original component props

#### Theme Integration
- [ ] **REQUIRED**: Use `const { theme } = useTheme()` hook
- [ ] **REQUIRED**: Replace all hardcoded colors with `theme.colors.*`
- [ ] **REQUIRED**: Replace all hardcoded spacing with `theme.spacing.*`
- [ ] **REQUIRED**: Replace all hardcoded border radius with `theme.borderRadius.*`
- [ ] **REQUIRED**: Use theme opacity values for transparency

#### Component Wrapping
- [ ] **REQUIRED**: Wrap original component with theme-aware styling
- [ ] **REQUIRED**: Pass all props through to original component
- [ ] **REQUIRED**: Apply theme-based styles via style prop or StyleSheet
- [ ] **REQUIRED**: Maintain component functionality and behavior
- [ ] **REQUIRED**: Preserve testIDs and accessibility props

#### Text Component Migration
- [ ] **REQUIRED**: Replace Text with ThemedText using appropriate variant
- [ ] **REQUIRED**: Replace TextInput with ThemedTextInput using appropriate variant
- [ ] **REQUIRED**: Remove fontFamily, fontSize, fontWeight, lineHeight from StyleSheet
- [ ] **REQUIRED**: Use ThemedText style prop for color overrides only
- [ ] **REQUIRED**: Document variant choices

#### Index File Completion
- [ ] **REQUIRED**: Update `index.ts` to export both components
- [ ] **REQUIRED**: Export original component: `export { ComponentName } from './ComponentName'`
- [ ] **REQUIRED**: Export themed component: `export { ThemedComponentName } from './ThemedComponentName'`
- [ ] **REQUIRED**: Export types if applicable: `export type { ComponentNameProps } from './ComponentName'`
- [ ] **REQUIRED**: Verify index file exports are correct

### Phase 5: Verification & Testing
**Quality Assurance**: Verify themed component works correctly and follows design system.

#### Theme Switching Verification
- [ ] **REQUIRED**: Verify component updates when theme changes
- [ ] **REQUIRED**: Test with all available themes
- [ ] **REQUIRED**: Verify colors update correctly
- [ ] **REQUIRED**: Verify spacing/border radius update if theme-specific
- [ ] **REQUIRED**: Verify no visual regressions

#### Design System Compliance
- [ ] **REQUIRED**: Verify component follows design directions file specifications
- [ ] **REQUIRED**: Verify design tokens from design directions file are used correctly
- [ ] **REQUIRED**: Verify border radius matches design system specifications from design directions
- [ ] **REQUIRED**: Verify spacing follows design system scale from design directions
- [ ] **REQUIRED**: Verify typography uses ThemedText variants per design directions

#### Code Quality Verification
- [ ] **REQUIRED**: Verify no hardcoded hex colors remain
- [ ] **REQUIRED**: Verify no hardcoded pixel values remain
- [ ] **REQUIRED**: Verify TypeScript compilation succeeds
- [ ] **REQUIRED**: Verify component props are properly typed
- [ ] **REQUIRED**: Verify accessibility is maintained

## Implementation Patterns

### Folder Structure Pattern
```
// Before
components/
  SearchBar.tsx

// After
components/
  SearchBar/
    index.ts              // Exports both components
    SearchBar.tsx         // Original component
    ThemedSearchBar.tsx   // Themed wrapper component
```

### Index File Pattern
```typescript
// SearchBar/index.ts
export { SearchBar } from './SearchBar';
export type { SearchBarProps } from './SearchBar';
export { ThemedSearchBar } from './ThemedSearchBar';
```

### Basic Themed Wrapper Pattern
```typescript
// SearchBar/ThemedSearchBar.tsx
import React from 'react';
import { View, StyleSheet } from 'react-native';
import { useTheme } from '../../context/ThemeContext';
import { SearchBar, SearchBarProps } from './SearchBar'; // Same folder import
import { ThemedText, ThemedTextInput } from '../common';

export function ThemedSearchBar(props: SearchBarProps) {
  const { theme } = useTheme();
  
  return (
    <SearchBar
      {...props}
      // Theme-based styles applied via wrapper or component modification
    />
  );
}
```

### Import Usage Pattern
```typescript
// ✅ CORRECT: Import from folder (uses index.ts)
import { SearchBar, ThemedSearchBar } from '../components/SearchBar';

// ✅ CORRECT: Import specific component
import { ThemedSearchBar } from '../components/SearchBar';

// ❌ FORBIDDEN: Direct file import (bypasses index.ts)
import { SearchBar } from '../components/SearchBar/SearchBar';
```

### Theme Value Replacement Pattern
```typescript
// ✅ CORRECT: Using theme values
const styles = StyleSheet.create({
  container: {
    backgroundColor: theme.colors.primary,        // ✅ From theme
    borderRadius: theme.borderRadius.card,        // ✅ From theme
    padding: theme.spacing.lg,                     // ✅ From theme
    margin: theme.spacing.md,                      // ✅ From theme
  },
  text: {
    color: theme.colors.textPrimary,               // ✅ From theme
    // fontSize, fontWeight, lineHeight removed - use ThemedText variant
  },
});

// ❌ FORBIDDEN: Hardcoded values
const badStyles = StyleSheet.create({
  container: {
    backgroundColor: '#70B9BE',  // ❌ Hardcoded - FORBIDDEN
    borderRadius: 16,             // ❌ Hardcoded - use theme.borderRadius.card
    padding: 24,                  // ❌ Hardcoded - use theme.spacing.lg
  },
  text: {
    color: '#0A2533',            // ❌ Hardcoded - FORBIDDEN
    fontSize: 16,                 // ❌ Use ThemedText variant instead
    fontWeight: '400',            // ❌ Use ThemedText variant instead
  },
});
```

### ThemedText Integration Pattern
```typescript
// ✅ CORRECT: Using ThemedText
import { ThemedText, ThemedTextInput } from '../common';

const ThemedComponent = () => {
  const { theme } = useTheme();
  
  return (
    <View>
      <ThemedText variant="title">Title</ThemedText>
      <ThemedText variant="body" style={{ color: theme.colors.textSecondary }}>
        Body text
      </ThemedText>
      <ThemedTextInput 
        variant="body"
        placeholder="Enter text"
        style={{ 
          backgroundColor: theme.colors.background,
          borderRadius: theme.borderRadius.mediumLarge,
        }}
      />
    </View>
  );
};

// ❌ FORBIDDEN: Direct Text/TextInput usage
const BadComponent = () => {
  return (
    <View>
      <Text style={{ fontSize: 22, fontWeight: '400' }}>Title</Text>
      <TextInput style={{ fontSize: 16, fontWeight: '400' }} />
    </View>
  );
};
```

### Opacity Pattern
```typescript
// ✅ CORRECT: Using theme opacity
const containerStyle = {
  backgroundColor: `${theme.colors.primary}${theme.opacity.l30}`, // 30% opacity
};

// ✅ CORRECT: Using rgba with theme colors
const rgb = hexToRgb(theme.colors.primary);
const containerStyle = {
  backgroundColor: `rgba(${rgb.r}, ${rgb.g}, ${rgb.b}, 0.3)`, // 30% opacity
};

// ❌ FORBIDDEN: Hardcoded opacity
const badStyle = {
  backgroundColor: 'rgba(112, 185, 190, 0.3)', // ❌ Hardcoded - FORBIDDEN
};
```

## Forbidden Patterns

### CRITICAL RESTRICTIONS
**These patterns are STRICTLY FORBIDDEN in themed components:**

- **FORBIDDEN**: Hardcoded hex colors like `#70B9BE`, `#0A2533`, etc.
- **FORBIDDEN**: Hardcoded pixel values for spacing (use `theme.spacing.*`)
- **FORBIDDEN**: Hardcoded border radius values (use `theme.borderRadius.*`)
- **FORBIDDEN**: Direct Text/TextInput usage (use ThemedText/ThemedTextInput)
- **FORBIDDEN**: fontFamily in StyleSheet (use ThemedText variants)
- **FORBIDDEN**: fontSize/fontWeight/lineHeight in StyleSheet (use ThemedText variants)
- **FORBIDDEN**: Hardcoded rgba colors (use theme colors + opacity)
- **FORBIDDEN**: Theme values that don't exist in theme system

## Usage Examples

### Basic Themed Component Creation
```
/create-themed-component for @SearchBar.tsx with @recipely-component-design-directions.plan.md
```

**Result Structure:**
```
components/
  SearchBar/
    index.ts              // Exports SearchBar and ThemedSearchBar
    SearchBar.tsx         // Original component (moved)
    ThemedSearchBar.tsx   // New themed wrapper
```

**Usage:**
```typescript
// Import from folder (uses index.ts)
import { SearchBar, ThemedSearchBar } from '../components/SearchBar';

// Use themed version
<ThemedSearchBar value={query} onChangeText={setQuery} />
```

### Themed Component with Custom Design Directions
```
/create-themed-component for @Button.tsx with @chefly-component-design-directions.plan.md
```

### Themed Component Migration
```
/create-themed-component for @FilterPanel.tsx with @recipely-component-design-directions.plan.md
```

## Success Criteria

### Themed Component Quality Checklist
**Every themed component must meet these standards:**

- [ ] **Folder Structure**: Component organized in folder with index.ts file
- [ ] **Index Exports**: Both original and themed components exported from index.ts
- [ ] **Import Paths**: All imports updated to use new folder structure
- [ ] **Theme Integration**: All styling values come from theme system
- [ ] **No Hardcoding**: Zero hardcoded hex colors, pixel values, or CSS values
- [ ] **Design System Compliance**: Follows design directions file specifications
- [ ] **ThemedText Usage**: Uses ThemedText/ThemedTextInput for all text rendering
- [ ] **Theme Switching**: Component updates automatically when theme changes
- [ ] **Functionality Preservation**: Original component functionality maintained
- [ ] **Type Safety**: Proper TypeScript types for all props
- [ ] **Accessibility**: Accessibility features preserved

### Themed Component Success Metrics
- **Zero Hardcoded Values**: 100% of styling from theme system
- **Theme Switching**: Component updates instantly on theme change
- **Design System Compliance**: 100% compliance with design directions
- **ThemedText Adoption**: 100% of text uses ThemedText/ThemedTextInput
- **Type Safety**: 100% TypeScript coverage with no errors

## Integration with Other Commands

### Pre-Creation
- Use with `/analyze-code-deep` to understand component structure before theming
- Use with `/understand` to understand design directions file before mapping

### Post-Creation
- Use with `/test` to verify themed component functionality
- Use with `/refactor` to improve themed component implementation
- Use with `/retro` to learn from themed component creation experience

**Remember**: The `/create-themed-component` command creates theme-aware wrapper components that enable rapid theme switching and design system compliance. All styling values must come from the theme system, and the component must follow the specifications in the provided design directions file.

