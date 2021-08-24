INSERT INTO "todo_item" ("title", "description", "completed", "due_date", "modified_on") VALUES
('Get milk', 'Make sure that it''s 2%', false, '2021-08-24', timezone('utc', now())),
('Brush my teeth', null, false, null, timezone('utc', now())),
('Change the oil', null, true, null, timezone('utc', now()))