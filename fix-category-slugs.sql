-- Fix category Slugs for re-added categories
-- Run this in your database to restore nav menu visibility.
-- The nav menu matches categories by Slug (finance, tech, etc.).

UPDATE Categories SET Slug = 'finance'   WHERE Id = 88;
UPDATE Categories SET Slug = 'tech'      WHERE Id = 89;
UPDATE Categories SET Slug = 'productivity' WHERE Id = 90;
UPDATE Categories SET Slug = 'software'  WHERE Id = 91;
UPDATE Categories SET Slug = 'other'     WHERE Id = 92;
UPDATE Categories SET Slug = 'investing' WHERE Id = 93;
UPDATE Categories SET Slug = 'debt-credit' WHERE Id = 94;
UPDATE Categories SET Slug = 'financial-apps' WHERE Id = 95;
UPDATE Categories SET Slug = 'reviews'   WHERE Id = 96;
UPDATE Categories SET Slug = 'gadgets'   WHERE Id = 97;
UPDATE Categories SET Slug = 'how-to'    WHERE Id = 98;
UPDATE Categories SET Slug = 'habits-routines' WHERE Id = 99;
UPDATE Categories SET Slug = 'home-office' WHERE Id = 100;
UPDATE Categories SET Slug = 'tools-templates' WHERE Id = 101;
UPDATE Categories SET Slug = 'apps'      WHERE Id = 102;
UPDATE Categories SET Slug = 'comparisons' WHERE Id = 103;
UPDATE Categories SET Slug = 'lifestyle' WHERE Id = 104;
UPDATE Categories SET Slug = 'career'    WHERE Id = 105;
