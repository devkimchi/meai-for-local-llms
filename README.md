# [Microsoft.Extensions.AI](https://learn.microsoft.com/dotnet/ai/microsoft-extensions-ai) for Local LLMs

This provides sample codes that uses Microsoft.Extensions.AI for locally running LLMs through [Docker Model Runner](https://docs.docker.com/ai/model-runner/), [Foundry Local](https://learn.microsoft.com/azure/ai-foundry/foundry-local/what-is-foundry-local), [Hugging Face](https://huggingface.co/) and [Ollama](https://ollama.com/).

> This is a trim-down version of [<img src="https://github.com/aliencube/open-chat-playground/raw/main/assets/icon-transparent.svg" alt="OpenChat Playground" width="16" /> OpenChat Playground](https://github.com/aliencube/open-chat-playground), specific to dealing with locally running LLMs. If you want to see more language models supported including MaaS and other vendors, try out [<img src="https://github.com/aliencube/open-chat-playground/raw/main/assets/icon-transparent.svg" alt="OpenChat Playground" width="16" /> OpenChat Playground](https://github.com/aliencube/open-chat-playground) instead.

## Prerequisites

- [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/) + [C# DevKit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) or [Visual Studio 2022 v17.14+](https://visualstudio.com/vs)
- [GitHub CLI](https://cli.github.com/)
- [PowerShell 7.5+](https://learn.microsoft.com/powershell/scripting/install/installing-powershell) 👉 Windows only
- [Docker Desktop](https://docs.docker.com/desktop/)
- [Foundry Local](https://learn.microsoft.com/azure/ai-foundry/foundry-local/get-started)
- [Ollama](https://ollama.com/download)

## Getting Started

### Get the repository ready

1. Login to GitHub.

    ```bash
    gh auth login
    ```

1. Check login status.

    ```bash
    gh auth status
    ```

1. Fork this repository to your account and clone the forked repository to your local machine.

    ```bash
    gh repo fork devkimchi/meai-for-local-llms --clone --default-branch-only
    ```

1. Navigate to the cloned repository.

    ```bash
    cd meai-for-local-llms
    ```

1. Get the repository root.

    ```bash
    # bash/zsh
    REPOSITORY_ROOT=$(git rev-parse --show-toplevel)
    ```

    ```powershell
    # PowerShell
    $REPOSITORY_ROOT = git rev-parse --show-toplevel
    ```

### Use GitHub Models

As a default, this app uses [GitHub Models](https://github.com/marketplace?type=models).

<details open>
  <summary><strong>With .NET Aspire</strong></summary>

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Add GitHub Personal Access Token (PAT) for GitHub Models connection. Make sure you should replace `{{YOUR_TOKEN}}` with your GitHub PAT.

    ```bash
    # bash/zsh
    dotnet user-secrets --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        set "Parameters:github-models-gh-apikey" "{{YOUR_TOKEN}}"
    ```

    ```bash
    # PowerShell
    dotnet user-secrets --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        set "Parameters:github-models-gh-apikey" "{{YOUR_TOKEN}}"
    ```

    > For more details about GitHub PAT, refer to the doc, [Managing your personal access tokens](https://docs.github.com/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

1. Run the app. The default language model is `openai/gpt-4o-mini`.

    ```bash
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost
    ```

   If you want to change the language model, add the `--model` option with a preferred model name. You can find the language model from the [GitHub Models catalog page](https://github.com/marketplace?type=models).

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type GitHubModels --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type GitHubModels --model <model-name>
    ```

1. Once the .NET Aspire dashboard opens, click navigate to `https://localhost:45160`, and enter prompts.

</details>

<details>
  <summary><strong>Without .NET Aspire</strong></summary>

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Add GitHub Personal Access Token (PAT) for GitHub Models connection. Make sure you should replace `{{YOUR_TOKEN}}` with your GitHub PAT.

    ```bash
    # bash/zsh
    dotnet user-secrets --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        set GitHubModels:Token "{{YOUR_TOKEN}}"
    ```

    ```bash
    # PowerShell
    dotnet user-secrets --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        set GitHubModels:Token "{{YOUR_TOKEN}}"
    ```

    > For more details about GitHub PAT, refer to the doc, [Managing your personal access tokens](https://docs.github.com/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

1. Run the app. The default language model is `openai/gpt-4o-mini`.

    ```bash
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp
    ```

   If you want to change the language model, add the `--model` option with a preferred model name. You can find the language model from the [GitHub Models catalog page](https://github.com/marketplace?type=models).

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type GitHubModels --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type GitHubModels --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

</details>

### Use Docker Model Runner

<details open>
  <summary><strong>With .NET Aspire</strong></summary>

1. Make sure Docker Desktop is up and running.

    ```bash
    docker info
    ```

1. Download language model, `ai/gpt-oss`, to your local machine.

    ```bash
    docker model pull ai/gpt-oss
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `DockerModelRunner` value. The default language model is `ai/gpt-oss`.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type DockerModelRunner
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type DockerModelRunner
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type DockerModelRunner --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type DockerModelRunner --model <model-name>
    ```

1. Once the .NET Aspire dashboard opens, click navigate to `https://localhost:45160`, and enter prompts.

</details>

<details>
  <summary><strong>Without .NET Aspire</strong></summary>

1. Make sure Docker Desktop is up and running.

    ```bash
    docker info
    ```

1. Download language model, `ai/gpt-oss`, to your local machine.

    ```bash
    docker model pull ai/gpt-oss
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `DockerModelRunner` value. The default language model is `ai/gpt-oss`.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type DockerModelRunner
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type DockerModelRunner
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type DockerModelRunner --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type DockerModelRunner --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

</details>

### Use Foundry Local

<details open>
  <summary><strong>With .NET Aspire</strong></summary>

1. Make sure Foundry Local is NOT running.

    ```bash
    foundry service status
    ```

   If Foundry Local service is up and running, run the following command:

    ```bash
    foundry service stop
    ```

1. Download language model, `gpt-oss-20b`, to your local machine.

    ```bash
    foundry model download gpt-oss-20b
    ```

   Once you download a language model, the foundry service is automatically up and running. If the service is up and running, stop it first.

    ```bash
    foundry service stop
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `FoundryLocal` value. The default language model is `gpt-oss-20b`.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type FoundryLocal
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type FoundryLocal
    ```

   If you want to change the language model, add the `--alias` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type FoundryLocal --alias <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type FoundryLocal --model <model-name>
    ```

1. Once the .NET Aspire dashboard opens, click navigate to `https://localhost:45160`, and enter prompts.

</details>

<details>
  <summary><strong>Without .NET Aspire</strong></summary>

1. Download language model, `gpt-oss-20b`, to your local machine.

    ```bash
    foundry model download gpt-oss-20b
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `FoundryLocal` value. The default language model is `gpt-oss-20b`.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type FoundryLocal
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type FoundryLocal
    ```

   If you want to change the language model, add the `--alias` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type FoundryLocal --alias <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type FoundryLocal --alias <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

</details>

### Use Hugging Face

Models from Hugging Face are running through Ollama server.

<details open>
  <summary><strong>With .NET Aspire</strong></summary>

With .NET Aspire, it uses the [ollama container image](https://hub.docker.com/r/ollama/ollama). Therefore, there's no need to run the Ollama server on your local machine.

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `HuggingFace` value. The default language model is `hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF`.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type HuggingFace
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type HuggingFace
    ```

   If you want to change the language model, add the `--model` option with a preferred model name. Make sure that the model name format MUST follow `hf.co/{ORG_NAME}/{MODEL_NAME}`, and the model name MUST be formatted in **GGUF**.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type HuggingFace --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type HuggingFace --model <model-name>
    ```

1. Once the .NET Aspire dashboard opens, click navigate to `https://localhost:45160`, and enter prompts.

</details>

<details>
  <summary><strong>Without .NET Aspire</strong></summary>

1. Make sure Ollama is up and running.

    ```bash
    ollama start
    ```

1. In a separate terminal, download language model, `LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF`, to your local machine.

    ```bash
    ollama pull hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `HuggingFace` value. The default language model is `hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF`.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type HuggingFace
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type HuggingFace
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type HuggingFace --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type HuggingFace --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

</details>

#### Use Ollama

<details open>
  <summary><strong>With .NET Aspire</strong></summary>

With .NET Aspire, it uses the [ollama container image](https://hub.docker.com/r/ollama/ollama). Therefore, there's no need to run the Ollama server on your local machine.

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `Ollama` value. The default language model is `gpt-oss`.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type Ollama
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type Ollama
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost \
        -- --connector-type Ollama --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet watch run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.AppHost `
        -- --connector-type Ollama --model <model-name>
    ```

1. Once the .NET Aspire dashboard opens, click navigate to `https://localhost:45160`, and enter prompts.

</details>

<details>
  <summary><strong>Without .NET Aspire</strong></summary>

1. Make sure Ollama is up and running.

    ```bash
    ollama start
    ```

1. In a separate terminal, download language model, `gpt-oss`, to your local machine.

    ```bash
    ollama pull gpt-oss
    ```

1. Make sure you are at the repository root.

    ```bash
    cd $REPOSITORY_ROOT
    ```

1. Run the app using the `--connector-type` option with the `Ollama` value. The default language model is `gpt-oss`.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type Ollama
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type Ollama
    ```

   If you want to change the language model, add the `--model` option with a preferred model name.

    ```bash
    # bash/zsh
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp \
        -- --connector-type Ollama --model <model-name>
    ```

    ```powershell
    # PowerShell
    dotnet run --project $REPOSITORY_ROOT/src/MEAIForLocalLLMs.WebApp `
        -- --connector-type Ollama --model <model-name>
    ```

1. Open your web browser, navigate to `http://localhost:5160`, and enter prompts.

</details>
