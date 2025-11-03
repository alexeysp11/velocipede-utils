-- A set of functions to simplify working with the "OrganizationItems" table.

CREATE OR REPLACE FUNCTION "GetParentOrganizationItem"(aItemId bigint)
RETURNS TABLE (
    "Id" bigint,
    "ItemType" integer,
    "IsDeleted" boolean,
    "HardDelete" boolean,
    "ParentItemId" bigint,
    "Address" text,
    "Uid" text,
    "Name" text,
    "Description" text,
    "DateCreated" timestamp with time zone,
    "DateChanged" timestamp with time zone,
    "BusinessEntityStatus" integer,
    "OrganizationName" text
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        poi."Id",
        poi."ItemType",
        poi."IsDeleted",
        poi."HardDelete",
        poi."ParentItemId",
        poi."Address",
        poi."Uid",
        poi."Name",
        poi."Description",
        poi."DateCreated",
        poi."DateChanged",
        poi."BusinessEntityStatus",
        poi."OrganizationName"
    FROM
        "OrganizationItems" oi
    LEFT JOIN 
        "OrganizationItems" poi on poi."Id" = oi."ParentItemId"
    WHERE
        oi."Id" = aItemId;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION "GetParentJobPosition"(aItemId bigint)
RETURNS TABLE (
    "Id" bigint,
    "ItemType" integer,
    "IsDeleted" boolean,
    "HardDelete" boolean,
    "ParentItemId" bigint,
    "Address" text,
    "Uid" text,
    "Name" text,
    "Description" text,
    "DateCreated" timestamp with time zone,
    "DateChanged" timestamp with time zone,
    "BusinessEntityStatus" integer,
    "OrganizationName" text
) AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE org_tree AS (
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        WHERE
            oi."Id" = aItemId
        UNION ALL
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        JOIN
            org_tree ot ON oi."Id" = ot."ParentItemId"
        WHERE
            ot."ItemType" != 1
    )
    SELECT
        org_tree."Id",
        org_tree."ItemType",
        org_tree."IsDeleted",
        org_tree."HardDelete",
        org_tree."ParentItemId",
        org_tree."Address",
        org_tree."Uid",
        org_tree."Name",
        org_tree."Description",
        org_tree."DateCreated",
        org_tree."DateChanged",
        org_tree."BusinessEntityStatus",
        org_tree."OrganizationName"
    FROM
        org_tree
    WHERE
        org_tree."ItemType" = 1
    ORDER BY
        org_tree."Id" DESC
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION "GetParentTeam"(aItemId bigint)
RETURNS TABLE (
    "Id" bigint,
    "ItemType" integer,
    "IsDeleted" boolean,
    "HardDelete" boolean,
    "ParentItemId" bigint,
    "Address" text,
    "Uid" text,
    "Name" text,
    "Description" text,
    "DateCreated" timestamp with time zone,
    "DateChanged" timestamp with time zone,
    "BusinessEntityStatus" integer,
    "OrganizationName" text
) AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE org_tree AS (
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        WHERE
            oi."Id" = aItemId
        UNION ALL
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        JOIN
            org_tree ot ON oi."Id" = ot."ParentItemId"
        WHERE
            ot."ItemType" != 4
    )
    SELECT
        org_tree."Id",
        org_tree."ItemType",
        org_tree."IsDeleted",
        org_tree."HardDelete",
        org_tree."ParentItemId",
        org_tree."Address",
        org_tree."Uid",
        org_tree."Name",
        org_tree."Description",
        org_tree."DateCreated",
        org_tree."DateChanged",
        org_tree."BusinessEntityStatus",
        org_tree."OrganizationName"
    FROM
        org_tree
    WHERE
        org_tree."ItemType" = 4
    ORDER BY
        org_tree."Id" DESC
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION "GetParentDepartment"(aItemId bigint)
RETURNS TABLE (
    "Id" bigint,
    "ItemType" integer,
    "IsDeleted" boolean,
    "HardDelete" boolean,
    "ParentItemId" bigint,
    "Address" text,
    "Uid" text,
    "Name" text,
    "Description" text,
    "DateCreated" timestamp with time zone,
    "DateChanged" timestamp with time zone,
    "BusinessEntityStatus" integer,
    "OrganizationName" text
) AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE org_tree AS (
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        WHERE
            oi."Id" = aItemId
        UNION ALL
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        JOIN
            org_tree ot ON oi."Id" = ot."ParentItemId"
        WHERE
            ot."ItemType" != 0
    )
    SELECT
        org_tree."Id",
        org_tree."ItemType",
        org_tree."IsDeleted",
        org_tree."HardDelete",
        org_tree."ParentItemId",
        org_tree."Address",
        org_tree."Uid",
        org_tree."Name",
        org_tree."Description",
        org_tree."DateCreated",
        org_tree."DateChanged",
        org_tree."BusinessEntityStatus",
        org_tree."OrganizationName"
    FROM
        org_tree
    WHERE
        org_tree."ItemType" = 0
    ORDER BY
        org_tree."Id" DESC
    LIMIT 1;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION "GetRootOrganizationItems"(aItemId bigint)
RETURNS TABLE (
    "Id" bigint,
    "ItemType" integer,
    "IsDeleted" boolean,
    "HardDelete" boolean,
    "ParentItemId" bigint,
    "Address" text,
    "Uid" text,
    "Name" text,
    "Description" text,
    "DateCreated" timestamp with time zone,
    "DateChanged" timestamp with time zone,
    "BusinessEntityStatus" integer,
    "OrganizationName" text
) AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE org_tree AS (
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        WHERE
            oi."Id" = aItemId
        UNION ALL
        SELECT
            oi."Id",
            oi."ItemType",
            oi."IsDeleted",
            oi."HardDelete",
            oi."ParentItemId",
            oi."Address",
            oi."Uid",
            oi."Name",
            oi."Description",
            oi."DateCreated",
            oi."DateChanged",
            oi."BusinessEntityStatus",
            oi."OrganizationName"
        FROM
            "OrganizationItems" oi
        JOIN
            org_tree ot ON oi."Id" = ot."ParentItemId"
    )
    SELECT
        org_tree."Id",
        org_tree."ItemType",
        org_tree."IsDeleted",
        org_tree."HardDelete",
        org_tree."ParentItemId",
        org_tree."Address",
        org_tree."Uid",
        org_tree."Name",
        org_tree."Description",
        org_tree."DateCreated",
        org_tree."DateChanged",
        org_tree."BusinessEntityStatus",
        org_tree."OrganizationName"
    FROM
        org_tree
    WHERE
        org_tree."ParentItemId" IS NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION "GetOrganizationByItemId"(aItemId bigint)
RETURNS TABLE (
    "Id" bigint,
    "CompanyId" bigint,
    "HeadItemId" bigint,
    "Uid" text,
    "Name" text,
    "Description" text,
    "DateCreated" timestamp with time zone,
    "DateChanged" timestamp with time zone,
    "BusinessEntityStatus" integer
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        o."Id",
        o."CompanyId",
        o."HeadItemId",
        o."Uid",
        o."Name",
        o."Description",
        o."DateCreated",
        o."DateChanged",
        o."BusinessEntityStatus"
    FROM "Organizations" o
    INNER JOIN "GetRootOrganizationItems"(aItemId) oi on o."HeadItemId" = oi."Id";
END;
$$ LANGUAGE plpgsql;
