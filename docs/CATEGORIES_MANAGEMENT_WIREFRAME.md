# Categories Management UI - Wireframe

## Overview
Categories Management page for authenticated users to create, edit, delete, and organize categories hierarchically.

**Route:** `/admin/categories`  
**Authentication:** Required (`[Authorize]`)

---

## Page Layout

```
┌─────────────────────────────────────────────────────────────┐
│ Header                                                      │
│ [☰] ShireBudgeters                                          │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  Sidebar          │  Main Content                            │
│  ┌──────────────┐ │  ┌──────────────────────────────────┐  │
│  │ Dashboard    │ │  │ Categories Management            │  │
│  │ Categories ⭐│ │  │                                  │  │
│  │ Posts        │ │  │ [+ Create] [🔍 Search] [Filter] │  │
│  │ Lead Magnets │ │  │                                  │  │
│  └──────────────┘ │  │ ┌──────┐ ┌──────┐ ┌──────┐     │  │
│                   │  │ │ Card │ │ Card │ │ Card │     │  │
│                   │  │ └──────┘ └──────┘ └──────┘     │  │
│                   │  │                                  │  │
│                   │  └──────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

---

## Main Page

### Header Section
```
┌─────────────────────────────────────────────────────────────┐
│ Categories Management                                        │
│ Manage your categories and organize them hierarchically     │
└─────────────────────────────────────────────────────────────┘
```

### Action Bar
```
┌─────────────────────────────────────────────────────────────┐
│ [+ Create Category]  [🔍 Search...]  [Filter: All ▼]  [Grid ▼] │
└─────────────────────────────────────────────────────────────┘
```

### Category Cards (Grid View)
```
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│ [Color Bar]  │  │ [Color Bar]  │  │ [Color Bar]  │
│              │  │              │  │              │
│ Category Name│  │ Category Name│  │ Category Name│
│ Description  │  │ Description  │  │ Description  │
│              │  │              │  │              │
│ 📁 3 children│  │ 📁 0 children│  │ 📁 1 child   │
│ Active       │  │ Active       │  │ Inactive     │
│              │  │              │  │              │
│ [Edit] [Del] │  │ [Edit] [Del] │  │ [Edit] [Del] │
└──────────────┘  └──────────────┘  └──────────────┘
```

### Category Card Details
- **Color indicator** (top bar)
- **Category name** (bold)
- **Description** (optional, truncated)
- **Children count** (folder icon + number)
- **Status badge** (Active/Inactive)
- **Action buttons** (Edit, Delete)

---

## Create Category Dialog

```
┌─────────────────────────────────────────────────────────────┐
│ Create Category                                        [×]  │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ Category Name *                                             │
│ ┌────────────────────────────────────────────────────────┐ │
│ │ [Enter category name...]                                │ │
│ └────────────────────────────────────────────────────────┘ │
│                                                              │
│ Description                                                 │
│ ┌────────────────────────────────────────────────────────┐ │
│ │ [Enter description (optional)...]                      │ │
│ │                                                         │ │
│ └────────────────────────────────────────────────────────┘ │
│                                                              │
│ Color                                                       │
│ [Color Picker] [#FF5733] [Preview: ███]                    │
│                                                              │
│ Parent Category                                             │
│ ┌────────────────────────────────────────────────────────┐ │
│ │ [Select parent category...] ▼                         │ │
│ └────────────────────────────────────────────────────────┘ │
│                                                              │
│ ☑ Active                                                    │
│                                                              │
│                    [Cancel]  [Create]                       │
└─────────────────────────────────────────────────────────────┘
```

**Fields:**
- **Name** (required, max 100 chars)
- **Description** (optional, max 500 chars)
- **Color** (color picker + hex input)
- **Parent Category** (dropdown, optional)
- **Active** (checkbox, default: checked)

---

## Edit Category Dialog

Same as Create Dialog, but:
- Title: "Edit Category"
- Pre-filled with existing data
- Button: "Update" instead of "Create"
- Shows audit info (Created Date, Modified Date) in read-only section

---

## Delete Confirmation Dialog

```
┌─────────────────────────────────────────────────────────────┐
│ Delete Category                                        [×]  │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ ⚠️  Are you sure you want to delete this category?          │
│                                                              │
│ Category: Finance                                           │
│                                                              │
│ ⚠️  Warning: This category has 3 child categories.         │
│     You must delete or reassign them first.                │
│                                                              │
│                    [Cancel]  [Delete]                        │
└─────────────────────────────────────────────────────────────┘
```

**Validation:**
- Cannot delete if has child categories
- Shows warning message
- Delete button disabled if has children

---

## Tree View

```
┌─────────────────────────────────────────────────────────────┐
│ 📁 Finance (Active)                    [Edit] [Delete]      │
│   ├─ 📁 Investments (Active)          [Edit] [Delete]      │
│   │   ├─ 📄 Stocks (Active)           [Edit] [Delete]      │
│   │   └─ 📄 Bonds (Active)            [Edit] [Delete]      │
│   └─ 📁 Budgeting (Active)            [Edit] [Delete]      │
│       └─ 📄 Monthly Budget (Active)    [Edit] [Delete]      │
│                                                              │
│ 📁 Technology (Active)                 [Edit] [Delete]      │
└─────────────────────────────────────────────────────────────┘
```

**Features:**
- Hierarchical display
- Expand/collapse nodes
- Indentation shows parent-child relationship
- Actions available on each node

---

## Empty State

```
┌─────────────────────────────────────────────────────────────┐
│                                                              │
│                    📁                                        │
│                                                              │
│            No Categories Found                             │
│                                                              │
│   Get started by creating your first category               │
│                                                              │
│            [+ Create Category]                              │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

---

## User Flows

### Create Category
1. Click "Create Category" button
2. Fill form (Name required)
3. Click "Create"
4. Success → Dialog closes, list refreshes
5. Error → Error message shown

### Edit Category
1. Click "Edit" on category card
2. Modify fields in dialog
3. Click "Update"
4. Success → Dialog closes, list refreshes

### Delete Category
1. Click "Delete" on category card
2. Confirm in dialog
3. If has children → Warning shown, delete disabled
4. If no children → Delete confirmed
5. Success → List refreshes

### Search & Filter
1. Type in search box → Real-time filter
2. Select filter (All/Active/Inactive) → Filter by status
3. Change view (Grid/List/Tree) → Display updates

---

## Key Features

- **CRUD Operations**: Create, Read, Update, Delete
- **Hierarchical Structure**: Parent-child relationships
- **Color Coding**: Visual category identification
- **Search**: Real-time filtering
- **Multiple Views**: Grid, List, Tree
- **Status Toggle**: Active/Inactive
- **Validation**: Client and server-side
- **Ownership**: Users only see/manage their own categories

---

## Technical Notes

- **Component**: `Categories.razor` at `/admin/categories`
- **Service**: `ICategoryService` for API calls
- **Authentication**: `[Authorize]` attribute required
- **Validation**: Name (1-100 chars), Description (max 500 chars), Color (hex/CSS)
- **API Endpoints**: GET, POST, PUT, DELETE `/api/categories`

---

## Responsive Design

**Mobile (< 768px):**
- Single column grid
- Full-width cards
- Stacked action buttons

**Tablet (768px - 1024px):**
- 2 column grid
- Collapsible sidebar

**Desktop (> 1024px):**
- 3-4 column grid
- Full sidebar visible

---

**End of Wireframe**
