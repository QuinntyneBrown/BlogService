import { Category } from "./category.model";

export const categoryActions = {
    ADD: "[Category] Add",
    EDIT: "[Category] Edit",
    DELETE: "[Category] Delete",
    CATEGORYS_CHANGED: "[Category] Categorys Changed"
};

export class CategoryEvent extends CustomEvent {
    constructor(eventName:string, category: Category) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { category }
        });
    }
}

export class CategoryAdd extends CategoryEvent {
    constructor(category: Category) {
        super(categoryActions.ADD, category);        
    }
}

export class CategoryEdit extends CategoryEvent {
    constructor(category: Category) {
        super(categoryActions.EDIT, category);
    }
}

export class CategoryDelete extends CategoryEvent {
    constructor(category: Category) {
        super(categoryActions.DELETE, category);
    }
}

export class CategorysChanged extends CustomEvent {
    constructor(categorys: Array<Category>) {
        super(categoryActions.CATEGORYS_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { categorys }
        });
    }
}
