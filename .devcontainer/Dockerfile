#https://medium.com/@ls12styler/docker-as-an-integrated-development-environment-95bc9b01d2c1

FROM amd64/ubuntu

# --> LINUX

RUN apt-get -y update

#zsh
RUN apt-get install -y zsh \
    && chsh -s $(which zsh)

#git
RUN apt-get install -y git \
    && git clone --depth=1 https://github.com/romkatv/powerlevel10k ~/powerlevel10k \
    && git clone --depth=1 https://github.com/HubiBoar/dotfiles ~/dotfiles \
    && git clone --depth=1 https://github.com/zsh-users/zsh-autosuggestions ~/.zsh/zsh-autosuggestions

#stow
RUN apt-get install -y stow \
    && stow --dir="$HOME/dotfiles" --adopt .

#docker
#docker run --privileged -v "/var/run/docker.sock:/var/run/docker.sock"
RUN apt-get install -y docker
RUN apt-get update && \
    apt-get -qy full-upgrade && \
    apt-get install -qy curl && \
    curl -sSL https://get.docker.com/ | sh


# --> DOTNET

#winget
RUN apt-get install -y wget \
    && wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb

#dotnet 7.0 and 8.0
RUN apt-get install -y dotnet-sdk-7.0 \
    && apt-get install -y dotnet-runtime-7.0 \
    && apt-get install -y aspnetcore-runtime-7.0 \
    && apt-get install -y dotnet-sdk-8.0 \
    && apt-get install -y dotnet-runtime-8.0 \
    && apt-get install -y aspnetcore-runtime-8.0

#azure CLI
RUN apt-get install -y curl && curl -L https://aka.ms/InstallAzureCLIDeb | bash