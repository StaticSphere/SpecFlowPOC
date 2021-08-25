INSERT INTO "todo_item" ("title", "description", "completed", "due_date", "modified_on") VALUES
('Get milk', 'Make sure that it''s 2%', false, '2021-08-24', timezone('utc', now())),
('Brush my teeth', null, true, null, timezone('utc', now())),
('Change the oil', null, true, null, timezone('utc', now())),
('Walk the dog', null, false, null, timezone('utc', now())),
('Generate the financials report', null, false, null, timezone('utc', now())),
('Look over resume', null, true, null, timezone('utc', now())),
('File expense report', null, false, null, timezone('utc', now())),
('Wash the car', null, false, null, timezone('utc', now()));

INSERT INTO "todo_tag" ("title", "color", "modified_on") VALUES
('Chore', '#008800', timezone('utc', now())),
('Auto', '#880000', timezone('utc', now())),
('Work', '#000088', timezone('utc', now()));

INSERT INTO "todo_item_tags" ("todo_item_id", "todo_tag_id") VALUES
(1, 1),
(3, 1),
(3, 2),
(4, 1),
(5, 3),
(6, 3),
(7, 3),
(8, 1),
(8, 2);
