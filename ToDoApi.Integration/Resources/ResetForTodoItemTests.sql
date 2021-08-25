DELETE FROM todo_item_tags;

DELETE FROM todo_tag;
ALTER SEQUENCE todo_tag_id_seq RESTART;

DELETE FROM todo_item;
ALTER SEQUENCE todo_item_id_seq RESTART;
