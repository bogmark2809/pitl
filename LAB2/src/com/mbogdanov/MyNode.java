package com.mbogdanov;

class MyNode<T>{
    MyNode<T> nextNode;
    T data;

    MyNode(T data){
        this.data = data;
        nextNode = null;
    }

    public T getData() {
        return data;
    }

    public MyNode<T> getNext() {
        return nextNode;
    }

    public void setData(T data) {
        this.data = data;
    }

    public void setNext(MyNode<T> next) {
        this.nextNode = next;
    }
}
