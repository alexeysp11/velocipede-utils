INSERT INTO "WfApplication" ("Uid", "Name", "DateCreated", "DateChanged", "BusinessEntityStatus")
VALUES
    ('d7b0eb60-190e-439d-89e1-c78594ac7f0c', 'UnifiedBusinessPlatform', NOW(), NOW(), 1);

INSERT INTO "WfApplicationForm" ("Uid", "Name", "WfApplicationId", "DateCreated", "DateChanged", "BusinessEntityStatus")
VALUES
    (uuid_generate_v4(), 'Auth/MyAccount', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Auth/SignIn', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Documents/Internal', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Documents/KnowledgeBase', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Home/Index', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Home/Notifications', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Hrm/Absenses', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Hrm/Employees', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Hrm/NewVacation', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'Hrm/Hiring', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'OrganizationalStructure/BriefDescription', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'OrganizationalStructure/OrganizationDetails', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'OrganizationalStructure/OrganizationItemDetails', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'OrganizationalStructure/OrganizationItems', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), 'OrganizationalStructure/Organizations', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1),
    (uuid_generate_v4(), '_Layout', (SELECT MAX("Id") FROM "WfApplication" WHERE "Name" = 'UnifiedBusinessPlatform'), NOW(), NOW(), 1);
