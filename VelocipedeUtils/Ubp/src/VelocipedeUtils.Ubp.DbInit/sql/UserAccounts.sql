-- Create user accounts based on the records from the "Employees" table.
INSERT INTO "UserAccounts" (
  "Login", "Email", "PhoneNumber", "Password", "Status", "Uid", "Name", "Description", "DateCreated", "DateChanged", "BusinessEntityStatus"
)
SELECT 
  "FirstName" || '.' || "LastName",
  "FirstName" || '.' || "LastName" || '@example.com',
  "MobilePhone",
  "FirstName" || '.' || "LastName",
  0,
  "Uid",
  "FullName",
  "Description",
  "DateCreated",
  "DateChanged",
  "BusinessEntityStatus"
FROM "Employees";

-- Fill in the association table between "Employees" and "UserAccounts".
INSERT INTO "EmployeeUserAccounts" (
  "EmployeeId", "UserAccountId", "DateCreated", "Uid", "Name", "Description", "DateChanged", "BusinessEntityStatus"
)
SELECT 
  e."Id",
  ua."Id",
  NOW(),
  e."Uid",
  e."FullName",
  e."Description",
  e."DateChanged",
  e."BusinessEntityStatus"
FROM "Employees" e
JOIN "UserAccounts" ua ON e."FirstName" || '.' || e."LastName" = ua."Login";

-- Executives.
INSERT INTO "UserGroups" ("IsGroupByDefault", "IsSystem", "Uid", "Name", "Description", "DateCreated", "DateChanged", "BusinessEntityStatus")
VALUES (FALSE, FALSE, uuid_generate_v4(), 'Executives', 'Executives', NOW(), NOW(), 1);

-- Executives.
INSERT INTO "UserAccountGroups" ("UserAccountId", "UserGroupId", "Uid", "DateCreated", "DateChanged", "BusinessEntityStatus")
SELECT
  ua."Id",
  (SELECT "Id" FROM "UserGroups" WHERE "Name" = 'Executives' LIMIT 1),
  uuid_generate_v4(),
  NOW(),
  NOW(),
  1
FROM "UserAccounts" ua
INNER JOIN "EmployeeUserAccounts" eua ON eua."UserAccountId" = ua."Id"
INNER JOIN "Employees" e ON e."Id" = eua."EmployeeId"
INNER JOIN "EmployeeOrganizationItem" eoi ON eoi."EmployeesId" = e."Id"
INNER JOIN "OrganizationItems" oi ON oi."Id" = eoi."OrganizationItemsId"
WHERE (
    oi."Name" LIKE '%System Administrator%'
    OR oi."Name" LIKE '%Director%'
    OR oi."Name" LIKE '%CEO%'
    OR oi."Name" LIKE '%President%'
    OR oi."Name" = 'IT Infrastructure Manager'
  )
  AND NOT oi."Name" LIKE '%Art Director%'
ORDER BY oi."Name";
