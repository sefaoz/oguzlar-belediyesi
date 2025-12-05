export interface Menu {
    id: string;
    title: string;
    url: string;
    order: number;
    parentId?: string | null;
    isVisible: boolean;
    children?: Menu[];
    target?: '_self' | '_blank';
}
