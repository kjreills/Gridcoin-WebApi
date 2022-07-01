#!/bin/bash

# This script must be run with root privileges.
if [[ $EUID -ne 0 ]]; then
   echo "This script must be run as root"
   exit 1
fi

# Update & install prerequisites.
export DEBIAN_FRONTEND='noninteractive'
apt-get update && apt-get upgrade -y -o Dpkg::Options::="--force-confold"
apt-get install -y --no-install-recommends software-properties-common apt-transport-https curl unzip

# Add appropriate repo, update packages & install Gridcoin daemon.
dist=$(lsb_release -sc)
case $dist in
    jessie|stretch|buster)
            curl https://bintray.com/user/downloadSubjectPublicKey?username=bintray | sudo gpg --dearmor --output /etc/apt/trusted.gpg.d/bintray.gpg
            add-apt-repository -y "deb https://dl.bintray.com/gridcoin/deb $dist stable"
            apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 379CE192D401AB61
            apt-get update && apt-get install -y --no-install-recommends gridcoinresearchd ;;
    trusty|xenial|artful|bionic)
            add-apt-repository -y ppa:gridcoin/gridcoin-stable
            apt-get update && apt-get install -y --no-install-recommends gridcoinresearchd ;;
    *)
            echo "DISTRO NOT SUPPORTED" && exit ;;
esac

# Create gridcoin user & directories.
useradd -m gridcoin
runuser -l gridcoin -c 'mkdir /home/gridcoin/.GridcoinResearch'

# Generate random rpc user and password for conf.
randUser=$(cat /dev/urandom | tr -dc 'a-zA-Z0-9' | fold -w 32 | head -n 1)
randPass=$(cat /dev/urandom | tr -dc 'a-zA-Z0-9' | fold -w 32 | head -n 1)

# Create the gridcoinresearch.conf file.
runuser -l gridcoin -c 'touch /home/gridcoin/.GridcoinResearch/gridcoinresearch.conf'
runuser -l gridcoin -c "cat <<EOT >> /home/gridcoin/.GridcoinResearch/gridcoinresearch.conf
server=1
daemon=1
rpcport=9332
listen=1
rpcuser=$randUser
rpcpassword=$randPass
addnode=addnode-us-central.cycy.me
addnode=ch.gridcoin.pl
addnode=ec2-3-81-39-58.compute-1.amazonaws.com
addnode=fi.gridcoin.pl
addnode=grcnode.tahvok.com
addnode=gridcoin.network
addnode=gridhost.ddns.net
addnode=node.gridcoin.network
addnode=pl.gridcoin.pl
addnode=seeds.gridcoin.ifoggz-network.xyz
addnode=tarmoilves.eu
addnode=vancouver01.gridcoin.ifoggz-network.xyz
EOT"

# Pull the official snapshot.
runuser -l gridcoin -c 'cd /home/gridcoin/.GridcoinResearch && if [ ! -f "snapshot.zip" ]; then curl -O https://download.gridcoin.us/download/downloadstake/signed/snapshot.zip; fi'
runuser -l gridcoin -c 'cd /home/gridcoin/.GridcoinResearch && unzip -o snapshot.zip'

# Create an alias.
echo "alias grc='sudo -u gridcoin gridcoinresearchd -datadir=/home/gridcoin/.GridcoinResearch/'" >> ~/.bashrc

# Launch the daemon.
runuser -l gridcoin -c "gridcoinresearchd -datadir=/home/gridcoin/.GridcoinResearch/ &"
