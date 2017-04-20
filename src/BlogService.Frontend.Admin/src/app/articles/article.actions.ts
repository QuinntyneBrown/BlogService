import { Article } from "./article.model";

export const articleActions = {
    ADD: "[Article] Add",
    EDIT: "[Article] Edit",
    DELETE: "[Article] Delete",
    ARTICLES_CHANGED: "[Article] Articles Changed"
};

export class ArticleEvent extends CustomEvent {
    constructor(eventName:string, article: Article) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { article }
        });
    }
}

export class ArticleAdd extends ArticleEvent {
    constructor(article: Article) {
        super(articleActions.ADD, article);        
    }
}

export class ArticleEdit extends ArticleEvent {
    constructor(article: Article) {
        super(articleActions.EDIT, article);
    }
}

export class ArticleDelete extends ArticleEvent {
    constructor(article: Article) {
        super(articleActions.DELETE, article);
    }
}

export class ArticlesChanged extends CustomEvent {
    constructor(articles: Array<Article>) {
        super(articleActions.ARTICLES_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { articles }
        });
    }
}
