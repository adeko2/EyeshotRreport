# Eyeshot Report

## 1. Issue - Loading an entity takes an enormous amount of time and allocates GBs of memory.

[project](https://github.com/adeko2/EyeshotRreport/tree/master/EyeshotReport)
We (Adeko Technologies) have observed an abnormal behaviour during loading an entity produced from a dwg source.

- The app seems to be stuck in an infinite loop inside `design1.Entities.Add(blockReference)`
- The app continues to allocate memory with a high speed without slowing down. During our test case we reached out to 14 GB of memory.

We have two files in the project. One is the porblematic version of the same entity while the other one opens correctly. To load the erronious block use `LeftCtrl+E` and use `LeftCtrl+W` for the working 

The problematic entity is produced only if the process is started in ContentRendered event.
![image](https://github.com/user-attachments/assets/86456c87-1f48-4ca4-bc33-850154e94445)
