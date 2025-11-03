-- Script for filling Absenses table.

DO $$
DECLARE
    year INT;
    employee_id BIGINT;
    absence_count INT;
    days INT;
    start_date DATE;
    end_date DATE;
    created_date DATE;
    status_index INT;
BEGIN
    FOR year IN 2010..2024 LOOP
        -- Select employees who were at least 18 years old at the time of the year.
        FOR employee_id IN SELECT "Id"
        FROM "Employees"
        WHERE EXTRACT(YEAR FROM "BirthDate") <= year - 18 LOOP
            -- Create 2-3 vacations (Reason = 3).
            absence_count := random_between(2, 3);
            start_date := random_date(year, year);
            end_date := start_date + interval '14 days' - interval '1 day';

            FOR i IN 1..absence_count LOOP
                INSERT INTO "Absenses" (
                    "EmployeeId", "Reason", "Status", "DateStartActual", "DateEndActual",
                    "DateStartExpected", "DateEndExpected", "Uid", "DateCreated", "DateChanged", "BusinessEntityStatus"
                ) VALUES (
                    employee_id,
                    3,
                    CASE WHEN year = 2024 THEN 0 ELSE 1 END,
                    start_date,
                    end_date,
                    start_date,
                    end_date,
                    uuid_generate_v4(),
                    start_date - interval '14 days',
                    NOW(),
                    1
                );
                
                start_date := random_date(year, year);
                end_date := start_date + interval '14 days';
            END LOOP;

            -- Create of additional absences with different reasons (10, 12, 15, 16, 17, 20)
            IF random_between(0, 10) % 2 = 0 THEN
                absence_count := random_between(1, 3);
                FOR i IN 1..absence_count LOOP
                start_date := random_date(year, year);
                end_date := start_date + interval '7 days';
                status_index := random_between(1, 6);

                INSERT INTO "Absenses" (
                    "EmployeeId", "Reason", "Status", "DateStartActual", "DateEndActual",
                    "DateStartExpected", "DateEndExpected", "Uid", "DateCreated", "DateChanged", "BusinessEntityStatus"
                ) VALUES (
                    employee_id,
                    CASE
                        WHEN status_index = 1 THEN 10
                        WHEN status_index = 2 THEN 12
                        WHEN status_index = 3 THEN 15
                        WHEN status_index = 4 THEN 16
                        WHEN status_index = 5 THEN 17
                        WHEN status_index = 6 THEN 20
                        ELSE 10
                    END,
                    CASE WHEN year = 2024 THEN 0 ELSE 1 END,
                    start_date,
                    end_date,
                    start_date,
                    end_date,
                    uuid_generate_v4(),
                    start_date - interval '14 days',
                    NOW(),
                    1
                );
                END LOOP;
            END IF;
        END LOOP;
    END LOOP;
END;
$$;
