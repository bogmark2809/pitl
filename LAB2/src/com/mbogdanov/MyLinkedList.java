package com.mbogdanov;

public class MyLinkedList<T> {
    private MyNode<T> head;
    private int nodeCount;

    public MyLinkedList(){
        this.head = new MyNode<T>(null);
        this.nodeCount = -1;
    }

    public int length() {
        return nodeCount+1;
    }

    public boolean isEmpty() {
        return this.nodeCount == -1;
    }

    public void add(T data) {
        MyNode<T> tempNode = new MyNode<T>(data);
        MyNode<T> currentNode = head;

        while (currentNode.getNext() != null) {
            currentNode = currentNode.getNext();
        }
        currentNode.setNext(tempNode);
        this.nodeCount++;
    }

    public Object get(int index){
        if (index < 0)
            return null;
        MyNode currentNode = null;
        if (head != null) {
            currentNode = head.getNext();
            for (int i = 0; i < index; i++) {
                if (currentNode.getNext() == null)
                    return null;

                currentNode = currentNode.getNext();
            }
            return currentNode.getData();
        }
        return null;
    }
}

