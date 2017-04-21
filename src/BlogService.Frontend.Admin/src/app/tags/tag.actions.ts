import { Tag } from "./tag.model";

export const tagActions = {
    ADD: "[Tag] Add",
    EDIT: "[Tag] Edit",
    DELETE: "[Tag] Delete",
    TAGS_CHANGED: "[Tag] Tags Changed"
};

export class TagEvent extends CustomEvent {
    constructor(eventName:string, tag: Tag) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { tag }
        });
    }
}

export class TagAdd extends TagEvent {
    constructor(tag: Tag) {
        super(tagActions.ADD, tag);        
    }
}

export class TagEdit extends TagEvent {
    constructor(tag: Tag) {
        super(tagActions.EDIT, tag);
    }
}

export class TagDelete extends TagEvent {
    constructor(tag: Tag) {
        super(tagActions.DELETE, tag);
    }
}

export class TagsChanged extends CustomEvent {
    constructor(tags: Array<Tag>) {
        super(tagActions.TAGS_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { tags }
        });
    }
}
